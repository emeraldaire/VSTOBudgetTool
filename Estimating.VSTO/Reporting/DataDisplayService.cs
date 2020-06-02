using Estimating.ProgressReporter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Excel;
//using Microsoft.Office.Tools.Excel;
using System.Threading.Tasks;
using System.Windows.Forms;
using Estimating.ProgressReporter.Enums;
using System.Drawing;

namespace Estimating.VSTO.Reporting
{
    public class DataDisplayService
    {
        //EXCEL INTEROP OBJECTS
        Microsoft.Office.Interop.Excel.Application Excel;
        Microsoft.Office.Interop.Excel._Workbook reportWorkbook;
        Microsoft.Office.Interop.Excel._Worksheet reportWorksheet;
        Microsoft.Office.Interop.Excel.Range worksheetRange;

        //FORMATTING COLORS
        Color BudgetHeaderColor;
        Color BudgetRecordColor;
        Color ActualHeaderColor;
        Color ActualRecordColor;
        Color BudgetSummaryRowColor;
        Color ActualSummaryRowColor;


        //VSTO OBJECTS
        private ComparatorReport _comparatorReport { get; set; }
        private CostCodeReport _costCodeReport { get; set; }
        public DataDisplayService()
        {

        }

        //public static DataDisplayService LoadDataObject(ComparatorReport comparatorReport)
        //{
        //    DataDisplayService dataDisplayService = new DataDisplayService();
        //    dataDisplayService._comparatorReport = comparatorReport;

        //    return dataDisplayService;
        //}
        public void DisplayCostCodeReport(CostCodeReport costCodeReport)
        {

            if (costCodeReport != null)
            {
                //Workbook newWorkbook = Globals.ThisAddIn.Application.Workbooks.Add();
                Excel = Globals.ThisAddIn.Application;
                Excel.Visible = true;

                //Get a new workbook and add a single sheet.  The new sheet will automatically become the activesheet.
                reportWorkbook = (_Workbook)(Excel.Workbooks.Add());
                //reportWorksheet = (_Worksheet)reportWorkbook.Worksheets.Add();
                reportWorksheet = reportWorkbook.ActiveSheet;

                //Set colors
                InitializeColorScheme();

                //Get table headers.
                List<string> costReportHeaders = GenerateCostReportHeaders();

                //Write the table headers to the active worksheet.
                int headerRow = 2;
                for (int i = 1; i <= costReportHeaders.Count; i++)
                {
                    reportWorksheet.Cells[headerRow, i].Value = costReportHeaders[i - 1];
                    reportWorksheet.Cells[headerRow, i].Font.Bold = true;
                    reportWorksheet.Cells[headerRow, i].Font.Size = 14;
                    if (i <= 4)
                    {
                        reportWorksheet.Cells[headerRow, i].Interior.Color = BudgetHeaderColor;
                        reportWorksheet.Cells[headerRow, i].Font.Color = XlRgbColor.rgbWhite;
                    }
                    else
                    {
                        reportWorksheet.Cells[headerRow, i].Interior.Color = ActualHeaderColor;
                        //reportWorksheet.Cells[headerRow, i].Font.Color = XlRgbColor.rgbDarkGrey;
                        reportWorksheet.Cells[headerRow, i].Font.Color = XlRgbColor.rgbWhite;
                    }
                }

                //Initialize the incremental variable used for writing records to the worksheet.
                int recordRow = headerRow + 1;
                foreach (CostCodeResult cc in costCodeReport.CostCodeResults)
                {
                    //BUDGETED
                    //Activity-PhaseCode
                    reportWorksheet.Cells[recordRow, 1] = cc.PhaseCode;
                    //Units (Budgeted)
                    reportWorksheet.Cells[recordRow, 2] = cc.EstimatedUnitQuantity;
                    //UOM (type)
                    reportWorksheet.Cells[recordRow, 3] = GetUnitOfMeasurement(cc.UnitOfMeasurement);
                    //Hours (Budgeted)
                    reportWorksheet.Cells[recordRow, 4] = cc.BudgetedHours;

                    //ACTUAL
                    //Units (Actual)
                    reportWorksheet.Cells[recordRow, 5] = cc.ActualUnitQuantity;
                    //UOM (Actual)
                    reportWorksheet.Cells[recordRow, 6] = GetUnitOfMeasurement(cc.UnitOfMeasurement);
                    //% Complete
                    reportWorksheet.Cells[recordRow, 7] = cc.PercentComplete;
                    reportWorksheet.Cells[recordRow, 7].NumberFormat = "0.00%";

                    //Earned Hrs
                    reportWorksheet.Cells[recordRow, 8] = cc.EarnedHours;
                    //Actual Hrs
                    reportWorksheet.Cells[recordRow, 9] = cc.ActualHours;
                    //Earned/Actual
                    reportWorksheet.Cells[recordRow, 10] = cc.EarnedActualRatio;
                    reportWorksheet.Cells[recordRow, 10].NumberFormat = "0.00%";

                    //Hours (Projected)
                    //TODO: Discuss calculation with Grant

                    //Select the entire row for formatting
                    worksheetRange = reportWorksheet.Range[reportWorksheet.Cells[recordRow, 1], reportWorksheet.Cells[recordRow, 11]];
                    worksheetRange.Font.Size = 14;
                    worksheetRange.Font.Bold = false;

                    //Color the Budget area.
                    Range budgetedValuesRange = reportWorksheet.Range[reportWorksheet.Cells[recordRow, 1], reportWorksheet.Cells[recordRow, 4]];
                    budgetedValuesRange.Interior.Color = BudgetRecordColor;

                    //Color the Actual area.
                    Range actualValuesRange = reportWorksheet.Range[reportWorksheet.Cells[recordRow, 5], reportWorksheet.Cells[recordRow, 11]];
                    actualValuesRange.Interior.Color = ActualRecordColor;

                    //Increment the counter
                    recordRow += 1;

                }




                //Autofit all cells with content.
                reportWorksheet.Columns.AutoFit();
            }
        }

        /// <summary>
        /// Returns the list of headers for the cost code report. 
        /// </summary>
        /// <returns></returns>
        private List<string> GenerateCostReportHeaders()
        {
            return new List<string>()
            {
                "Activity",
                "Units (Budgeted)",
                "UOM",
                "Hours (Budgeted)",
                "Units (Actual)",
                "UOM (Actual)",
                "% Complete",
                "Earned Hrs",
                "Actual Hrs",
                "Earned/Actual",
                "Hours (Projected)"
            };
        }

        /// <summary>
        /// Converts the ENUM 'UnitOfMeasurement' into a formatted string suitable for display.
        /// </summary>
        /// <param name="unitOfMeasurement"></param>
        /// <returns></returns>
        private string GetUnitOfMeasurement(UnitOfMeasurement unitOfMeasurement)
        {
            switch (unitOfMeasurement)
            {
                case UnitOfMeasurement.LinealFeet:
                    return "LF";
                case UnitOfMeasurement.SquareFeet:
                    return "SQ FT";
                case UnitOfMeasurement.Each:
                    return "EA";
                case UnitOfMeasurement.LS:
                    return "LS";
                default:
                    return "Unknown";
            }
        }

        private void InitializeColorScheme()
        {
            BudgetHeaderColor = ColorTranslator.FromHtml("#D08100");
            BudgetRecordColor = ColorTranslator.FromHtml("#ffc76d");
            ActualHeaderColor = ColorTranslator.FromHtml("#5491b5"); 
            ActualRecordColor = ColorTranslator.FromHtml("#D8edfa"); 
            //BudgetSummaryRowColor = XlRgbColor.rgbLightGray;
            //ActualSummaryRowColor = ColorTranslator.FromHtml("231,231,231"); 
        }


    }
}