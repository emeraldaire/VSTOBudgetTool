using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Estimating.ProgressReporter.Services;
using Estimating.ProgressReporter.Model;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Tools.Ribbon;
using Estimating.VSTO.Reporting;

namespace Estimating.VSTO
{
    public partial class RibbonBudgetTab
    {
        private void RibbonBudgetTab_Load(object sender, RibbonUIEventArgs e)
        {
        }

        /// <summary>
        /// Opens a file dialog window for the user to select the CSV Field Report that will be processed.
        /// </summary>
        /// <remarks>
        /// Uses CSV filter because validation of file extensions has not currently been implemented in the processing
        /// methods.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImportFieldReport_Click(object sender, RibbonControlEventArgs e)
        {
            OpenFileDialog ImportFieldReportDialog = new OpenFileDialog()
            {
                Filter = "CSV Files (*.csv)|*.csv",
                Title = "Select a Field Report"
            };

            DialogResult importResult = ImportFieldReportDialog.ShowDialog();
            if(importResult == DialogResult.OK)
            {
                //TODO: Get jobnumber from user. 
                string jobNumber ="2170507";

                // MAIN PROGRAM
                // ***************************************************
                //1.  READ THE CSV FILE INTO MEMORY.

                //Get the selected file path. 
                string selectedFile = ImportFieldReportDialog.FileName;
                // Use the CSVDataService to process the file contents and produce a list of SystemReport objects.  
                


                // ***************************************************
                //2.  USE THE CLIENT REPORT SERVICE TO RUN AND RETURN THE REPORT OBJECT.
                //Note:  To get this far, the field report must have already been validated and processed into a List of SystemReport objects. 
                //Create the report service. 
                ClientReportService clientReportService = new ClientReportService(jobNumber);
                //Send the field report list to the report service to generate the full summary report object.  
                ComparatorReport finishedReport = clientReportService.GetReportSummary(GenerateFakeReportList());



                // ***************************************************
                //3.  DISPLAY THE PREFERRED DATA FROM THE REPORT OBJECT.
                //Send the report object to the data display service.
                DataDisplayService dataDisplayService = DataDisplayService.LoadDataObject(finishedReport);





            }
        }

        /// <summary>
        /// Opens a SaveFileDialog to save the current worksheet.
        /// </summary>
        /// <remarks>
        /// Should save the results shown in the current worksheet.  The intended use is for outputting the results that are automatically generated 
        /// when the user uploads a Field Report (the report is automatically run and the results are printed and formatted in a worksheet inside the 
        /// active workbook.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveCompletedReport_Click(object sender, RibbonControlEventArgs e)
        {
            SaveFileDialog SaveCurrentWeeklyReport = new SaveFileDialog()
            {
                Filter = "Excel file (*.xlsx)|*.xlsx",
                Title = "Save As"
            };

            DialogResult saveResult = SaveCurrentWeeklyReport.ShowDialog();
            if(saveResult == DialogResult.OK)
            {
                Globals.ThisAddIn.Application.ActiveWorkbook.SaveCopyAs(SaveCurrentWeeklyReport.FileName);
            }



        }

        /// <summary>
        /// Creates an Estimate record in the database.
        /// </summary>
        /// <remarks>
        /// DATE: 5/14/20
        /// AUTHOR: Noah B
        /// The data from the current estimate sheet must be formatted into the required interface before being committed to the database.  For now, the 
        /// only requirements will be the (minimal) data necessary to run the field report comparisons (see ProgressReporter utility for implementation)
        /// but this will be expanded later to accomodate future data needs.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveEstimateToDatabase_Click(object sender, RibbonControlEventArgs e)
        {
            MessageBox.Show("Saving Estimate Data");
            //TODO: Insert processing methods here OR call to the main processing method.  
        }

        #region "Dummy Code"
        private List<SystemReport> GenerateFakeReportList()
        {
            List<SystemReport> fakeReportedSystemList = new List<SystemReport>();

            SystemReport garagefans = new SystemReport("GF-1")
            {
                PhaseCodes = new List<PhaseCode>()
                {
                    new PhaseCode("0001-0701"),
                    new PhaseCode("0001-0601"),
                    new PhaseCode("0001-0401")
                }
            };

            //Partially complete; there were four phase codes in the estimate, but only two of them are being reported.
            SystemReport ETrashExhaust = new SystemReport("T-E")
            {
                PhaseCodes = new List<PhaseCode>()
                {
                    new PhaseCode("0001-0701"),
                    new PhaseCode("0001-0601"),
                    new PhaseCode("0001-0501")
                },

            };

            //Partially complete; there were two phase codes in the estimate, but only one of them is being reported.
            SystemReport ECorridorSupply = new SystemReport("RTU-1")
            {
                PhaseCodes = new List<PhaseCode>()
                {
                    new PhaseCode("0001-0701"),
                },

            };

            fakeReportedSystemList.Add(garagefans);
            fakeReportedSystemList.Add(ETrashExhaust);
            fakeReportedSystemList.Add(ECorridorSupply);

            return fakeReportedSystemList;

        }



        #endregion




    }
}
