using Estimating.ProgressReporter.Model;
using System;
using System.Collections.Generic;
using Microsoft.Office.Interop.Excel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Estimating.VSTO.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// DATE: 6/2/20
    /// AUTHOR: Noah B
    /// Estimate Helper is consumed by the VSTO Ribbon code-behind to manage the spreadsheet navigation and information gathering when the user 
    /// saves the current Estimate data to the database.
    /// </remarks>
    public class EstimateHelper
    {

        //EXCEL INTEROP OBJECTS
        Microsoft.Office.Interop.Excel.Application Excel;
        Microsoft.Office.Interop.Excel._Workbook estimateWorkbook;
        Microsoft.Office.Interop.Excel._Worksheet estimateWorksheet;
        Microsoft.Office.Interop.Excel.Range systemNameRange;

        //VSTO OBJECTS
        private List<SystemEstimate> systemEstimateList { get; set; }
        string equipmentHoursPhaseCode = "0001-0901";
        string buildingTradeHoursPhaseCode = "0001-0801";
        string RFRPipeHoursePhaseCode = "0001-0602";

        //The Estimate sheet is hardcoded to allow only 103 rows.  Rather than mucking around with Interop.Excel, it's easier (however inelegant) here to simply 
        //iterate through the entire range to detect the listed systems.  'maxRow' specifies the total number of rows over which the application will run. If the
        //dimensions of the spreadsheet change to accomodate more (very unlikely), adjust the value here.
        private int maxRow { get; set; } = 103;
        //Set the reference row; this is the first row on which the application will begin to detect spreadsheet entries.
        int targetRow = 3;
        //Set the column number where the system type entries are located. 
        private int systemTypeColumn = 2;
        //Column for 0901
        private int equipmentPhaseCodeColumn = 5;
        //Column for 0801
        private int buildingTradesPhaseCodeColumn = 9;
        //Column for 0602
        private int RFRPipePhaseCodeColumn = 12;

        public EstimateHelper()
        {
            //Workbook newWorkbook = Globals.ThisAddIn.Application.Workbooks.Add();
            Excel = Globals.ThisAddIn.Application;
            Excel.Visible = true;
            Excel.DisplayAlerts = false;

            estimateWorkbook = Excel.ActiveWorkbook;
            //reportWorksheet = (_Worksheet)reportWorkbook.Worksheets.Add();
            estimateWorksheet = estimateWorkbook.ActiveSheet;
        }
        /// <summary>
        /// Attempts to activate the 'MainForm' tab of the Estimate workbook.  If the attempt is successful, this function will return TRUE. 
        /// If not, an error message will be displayed to the user that includes directions on how to correctly activate the proper components.
        /// </summary>
        /// <returns></returns>
        public bool CalibratePosition()
        {
            //Check if the current worksheet is the "MainForm" tab. 
            if(estimateWorksheet.Name == "MainForm")
            {
                return true;
            }
            else
            {
                try
                {
                    estimateWorkbook.Sheets["MainForm"].Activate();
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Read through the 'MainForm' worksheet and detect the individual systems.  See Master Documentation for more information on how 
        /// individual systems must be arranged on the 'MainForm' worksheet.
        /// </summary>
        /// <remarks>
        /// 6/7/20 Implement Strategy #2 as outlined below (by system name)
        /// 
        /// //STRATEGY #1
        //Iterate through Column B.  When a non-empty cell is found, make a note. 

        //Count the number of non-empty cells below the first one.  In most cases, the system will not contain any further subdivisions
        //and the non-empty 'Type' cell will be bordered by a blank cell above or below. 

        //For each subdivision found, the hrs for the cost codes must be summed. 
        //Use the Row indices of the top and bottom subdivisions to sum the hours in the critical columns.

        //Create the SystemEstimate object and give it the name found in the 'System' column (Column A).

        //For each of the important phase codes (0901 - Column E, 0801 - Column I, 0602 - Column L), create a phase code object 
        //and assign the sum of the hours that was calculated above. 

        //Add the PhaseCode to the SystemEstimate.


        //STRATEGY #2 
        //Iterate through Column A (System Name)
        //Each system gets only one name.  When a non-empty cell is found, test the value of the cell one column to the right and one row down. 
        //If this cell is empty, there is only one subdivision to the system (most common).  If it is not empty, continue to query the cells below 
        //until an empty cell is found.  Each non-empty cell represents a single subdivision of the system name.  

        //Count the number of non-empty cells below the first one.  In most cases, the system will not contain any further subdivisions
        //and the non-empty 'Type' cell will be bordered by a blank cell above or below. 

        //For each subdivision found, the hrs for the cost codes must be summed. 
        //Use the Row indices of the top and bottom subdivisions to sum the hours in the critical columns.

        //Create the SystemEstimate object and give it the name found in the 'System' column (Column A).

        //For each of the important phase codes (0901 - Column E, 0801 - Column I, 0602 - Column L), create a phase code object 
        //and assign the sum of the hours that was calculated above. 

        //Add the PhaseCode to the SystemEstimate.
        /// 
        /// 
        /// </remarks>
        public List<SystemEstimate> PopulateSystemList()
        {
            systemEstimateList = new List<SystemEstimate>();
            try
            {

                for (int i = targetRow; i <= maxRow; i++)
                {
                    systemNameRange = estimateWorksheet.Cells[i, 1];
                    if (systemNameRange.Value2 != null && systemNameRange.Value2 != "" && HasTypeEntry(i))
                    {

                        //If the system is not divided, create a SystemEstimate object and add it to the List.
                        if (!HasSubdivisions(i))
                        {
                            //SystemEstimate currentSystem = GenerateUndividedSystem(systemNameRange.Value2, i);
                            systemEstimateList.Add(GenerateUndividedSystem(systemNameRange.Value2, i));
                        }
                        //If the system is divided, first process all the subdivisions.  Then create a SystemEstimate object and add it to the List.
                        else
                        {
                            systemEstimateList.Add(GeneratedDividedSystem(systemNameRange.Value2, i));
                        }
                        //bool dividedSystem = HasSubdivisions(i);
                        //MessageBox.Show("System: " + systemNameRange.Value2.ToString() + " Divided System: " + dividedSystem.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message.ToString());
                throw;
            }

            return systemEstimateList;
        }

        /// <summary>
        /// Returns TRUE if the system on the current row contains subdivision entries.
        /// </summary>
        /// <param name="currentRow"></param>
        /// <returns></returns>
        private bool HasSubdivisions(int currentRow)
        {
            //Examine the cell in the 'Description' column (column B) one row below the current row.  If it is non-empty, 
            //the system has subdivisions and must be handled accordingly.
            Range subdivisionEntry = estimateWorksheet.Cells[(currentRow + 1), systemTypeColumn];
            if(subdivisionEntry.Value2 != null && subdivisionEntry.Value2 != "")
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// Returns false if there is no entry in the 'type' column (Column 2) in the same row as that which contains the system name.
        /// This function is primarily for validating the required spreadsheet format.
        /// </summary>
        /// <param name="currentRow"></param>
        /// <returns></returns>
        private bool HasTypeEntry(int currentRow)
        {
            //Make sure there is a type entry in the same row as the system name.
            Range currentSystemType = estimateWorksheet.Cells[currentRow, systemTypeColumn];
            if(currentSystemType.Value2 == null || currentSystemType.Value2.ToString() == "")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Generates a SystemEstimate object for the undivided system located on the current row.
        /// </summary>
        /// <param name="currentRow"></param>
        /// <returns></returns>
        private SystemEstimate GenerateUndividedSystem(string systemName, int currentRow)
        {
            int equipmentHours = AssignHoursToObject(currentRow, equipmentPhaseCodeColumn);
            int buildingTradeHours = AssignHoursToObject(currentRow, buildingTradesPhaseCodeColumn);
            int RFRHours = AssignHoursToObject(currentRow, RFRPipePhaseCodeColumn);

            List<PhaseCode> phaseCodeList = new List<PhaseCode>()
            {
                new PhaseCode(equipmentHoursPhaseCode){ EstimatedHours = equipmentHours, AssociatedSystem = systemName},
                new PhaseCode(buildingTradeHoursPhaseCode) { EstimatedHours = buildingTradeHours, AssociatedSystem = systemName},
                new PhaseCode(RFRPipeHoursePhaseCode){EstimatedHours = RFRHours, AssociatedSystem = systemName}
            };

            return new SystemEstimate(systemName){ PhaseCodes = phaseCodeList };
        }
        
        /// <summary>
        /// Generates a SystemEstimate object for the divided system located on the current row.
        /// </summary>
        /// <param name="currentRow"></param>
        /// <returns></returns>
        private SystemEstimate GeneratedDividedSystem(string systemName, int currentRow)
        {
            int equipmentHours = GetSummedHours(currentRow, equipmentPhaseCodeColumn);
            int buildingTradeHours = GetSummedHours(currentRow, buildingTradesPhaseCodeColumn);
            int RFRHours = GetSummedHours(currentRow, RFRPipePhaseCodeColumn);

            List<PhaseCode> phaseCodeList = new List<PhaseCode>()
            {
                new PhaseCode(equipmentHoursPhaseCode){ EstimatedHours = equipmentHours, AssociatedSystem = systemName},
                new PhaseCode(buildingTradeHoursPhaseCode) { EstimatedHours = buildingTradeHours, AssociatedSystem = systemName},
                new PhaseCode(RFRPipeHoursePhaseCode){EstimatedHours = RFRHours, AssociatedSystem = systemName}
            };

            return new SystemEstimate(systemName) { PhaseCodes = phaseCodeList };
        }

        //Specify the cell where the phase code hours value is; if the value is null or blank, return 0.
        private int AssignHoursToObject(int currentRow, int phaseCodeColumn)
        {
            //systemNameRange = estimateWorksheet.Cells[i, 1];
            Range hourEntry = estimateWorksheet.Cells[currentRow, phaseCodeColumn];
            
            if(hourEntry.Value2 != null)
            {
                return(int)hourEntry.Value2;
            }
            else
            {
                //fuck you.
                return 0;
            }
        }

        /// <summary>
        /// Calculates the summed hours by phase code column for the divided system listed in the current row.
        /// </summary>
        /// <param name="currentRow"></param>
        /// <param name="phaseCodeColumn"></param>
        /// <returns></returns>
        private int GetSummedHours(int currentRow, int phaseCodeColumn)
        {
            //Initialize accumulator
            int summedHours = 0;
            int numberOfDivisions = GetNumberOfDivisions(currentRow);
            for (int i = 0; i < numberOfDivisions; i++)
            {
                //Start with the type on the current row, then offset by 'i' in the specified phaseCodeColumn to fetch the hours for each subdivision.
                summedHours += AssignHoursToObject((currentRow+i), phaseCodeColumn);
            }

            return summedHours;
        }
        /// <summary>
        /// Returns the number of non-null entries associated with the system listed in the current row.
        /// </summary>
        /// <param name="currentRow"></param>
        /// <returns></returns>
        private int GetNumberOfDivisions(int currentRow)
        {
            //Use a counter to keep track of how many subdivisions are non-null or non-empty;
            int numberOfDivisions = 0;
            for (int i = 0; i <= maxRow; i++)
            {
                Range divisionEntry = estimateWorksheet.Cells[(currentRow + i), systemTypeColumn];
                if (divisionEntry.Value2 != null && divisionEntry.Value2.ToString() != "")
                {
                    numberOfDivisions += 1;
                }
                else
                {
                    return numberOfDivisions; //Do nothing and proceed to the next row
                } 
            }

            return numberOfDivisions;
        }

    }
    
}
