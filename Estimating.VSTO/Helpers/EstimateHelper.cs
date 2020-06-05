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
        Microsoft.Office.Interop.Excel.Range systemRange;

        //VSTO OBJECTS
        private List<SystemEstimate> systemEstimateList { get; set; }
        
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
        public void PopulateSystemList()
        {

            int targetRow = 3;
            //int systemDescriptionColumn = 2;
            //The Estimate sheet is hardcoded to allow only 103 rows.  Rather than mucking around with Interop.Excel, it's easier (however inelegant) here to simply 
            //iterate through the entire range to detect the listed systems.
            int maxRow = 103;

            for(int i = targetRow; i <= maxRow; i++)
            {
                systemRange = estimateWorksheet.Cells[i, 1];
                if(systemRange.Value2 != null && systemRange.Value2 != "")
                {
                    //Range subDivision = estimateWorksheet.Cells[targetRow + 1, 2];

                    //Range nextCell = Excel.ActiveCell.Offset[1, 1].Select();
                    //MessageBox.Show(nextCell.Value2.ToString());
                    MessageBox.Show(systemRange.Value2.ToString() + " Row: " + i.ToString());
                }
            }

            //STRATEGY #1
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






        }




    }
    
}
