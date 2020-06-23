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
        Color OverProjection;
        Color UnderProjection;
        //Color BudgetSummaryRowColor;
        //Color ActualSummaryRowColor;
        //Color LargeHeaderColor;

        //FORMATTING VALUES
        int FontSizeHeader = 20;
        int RowHeightHeader = 26;
        
        //CELL REFERENCES
        int headerRow = 2;


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
                Excel.DisplayAlerts = false;
                
                //Get a new workbook and add a single sheet.  The new sheet will automatically become the activesheet.
                reportWorkbook = (_Workbook)(Excel.Workbooks.Add());
                //reportWorksheet = (_Worksheet)reportWorkbook.Worksheets.Add();
                reportWorksheet = reportWorkbook.ActiveSheet;

                //Set colors
                InitializeColorScheme();

                //Get table headers.
                List<string> costReportHeaders = GenerateCostReportHeaders();

                //Write the table headers to the active worksheet
              
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

                    //Select the entire row for formatting
                    worksheetRange = reportWorksheet.Range[reportWorksheet.Cells[recordRow, 1], reportWorksheet.Cells[recordRow, 12]];
                    worksheetRange.Font.Size = 14;
                    worksheetRange.Font.Bold = false;

                    //Color the Budget area.
                    Range budgetedValuesRange = reportWorksheet.Range[reportWorksheet.Cells[recordRow, 1], reportWorksheet.Cells[recordRow, 4]];
                    budgetedValuesRange.Interior.Color = BudgetRecordColor;

                    //Color the Actual area.
                    Range actualValuesRange = reportWorksheet.Range[reportWorksheet.Cells[recordRow, 5], reportWorksheet.Cells[recordRow, 12]];
                    actualValuesRange.Interior.Color = ActualRecordColor;

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
                    reportWorksheet.Cells[recordRow, 7].NumberFormat = "0%";

                    //Team Budget
                    reportWorksheet.Cells[recordRow, 8] = cc.ProjectedHours;

                    //Earned Hrs
                    reportWorksheet.Cells[recordRow, 9] = cc.EarnedHours;
                    //Actual Hrs
                    reportWorksheet.Cells[recordRow, 10] = cc.ActualHours;
                    //Earned/Actual
                    reportWorksheet.Cells[recordRow, 11] = cc.EarnedActualRatio;

                    //Apply conditional formatting
                    //Color the Actual area.
                    Range performanceValuesRange = reportWorksheet.Range[reportWorksheet.Cells[recordRow, 11], reportWorksheet.Cells[recordRow, 11]];
                    if (cc.EarnedActualRatio > 1)
                    {
                        performanceValuesRange.Interior.Color = OverProjection;
                    }
                    else
                    {
                        performanceValuesRange.Interior.Color = UnderProjection;
                    }

                    //Projected Hours
                    reportWorksheet.Cells[recordRow, 12] = Math.Round((cc.ProjectedHours * cc.EarnedActualRatio), 2);

                    //Hours (Projected)
                    //TODO: Discuss calculation with Grant
             
                    //Increment the counter
                    recordRow += 1;

                }

                //Create the merged cell headers; this can only happen if (headerRow >= 2). 
                if (headerRow >= 2)
                {
                    //BUDGET
                    Range BudgetValuesHeaderRange = reportWorksheet.Range[reportWorksheet.Cells[headerRow - 1, 1], reportWorksheet.Cells[headerRow - 1, 4]];
                    BudgetValuesHeaderRange.Value = "Budget";
                    BudgetValuesHeaderRange.Merge();
                    BudgetValuesHeaderRange.Interior.Color = ColorTranslator.FromHtml("#e3e3e3");
                    BudgetValuesHeaderRange.Font.Size = FontSizeHeader;
                    BudgetValuesHeaderRange.RowHeight = RowHeightHeader;
                    BudgetValuesHeaderRange.Font.Bold = true;
                    BudgetValuesHeaderRange.Style.HorizontalAlignment = XlHAlign.xlHAlignCenter;

                    //ACTUAL
                    Range ActualValuesHeaderRange = reportWorksheet.Range[reportWorksheet.Cells[headerRow - 1, 5], reportWorksheet.Cells[headerRow - 1, 10]];
                    ActualValuesHeaderRange.Value = "Actual";
                    ActualValuesHeaderRange.Merge();
                    ActualValuesHeaderRange.Interior.Color = ColorTranslator.FromHtml("#e3e3e3");
                    ActualValuesHeaderRange.Font.Size = FontSizeHeader;
                    ActualValuesHeaderRange.RowHeight = RowHeightHeader;
                    ActualValuesHeaderRange.Font.Bold = true;
                    ActualValuesHeaderRange.Style.HorizontalAlignment = XlHAlign.xlHAlignCenter;

                    //PRODUCTIVITY
                    reportWorksheet.Cells[headerRow - 1, 11] = "Productivity";
                    reportWorksheet.Cells[headerRow - 1, 11].Interior.Color = ColorTranslator.FromHtml("#e3e3e3");
                    reportWorksheet.Cells[headerRow - 1, 11].Font.Size = FontSizeHeader;
                    reportWorksheet.Cells[headerRow - 1, 11].Font.Bold = true;
                    reportWorksheet.Cells[headerRow - 1, 11].RowHeight = RowHeightHeader;
                    reportWorksheet.Cells[headerRow - 1, 11].Style.HorizontalAlignment = XlHAlign.xlHAlignCenter;

                    //PROJECTION
                    reportWorksheet.Cells[headerRow - 1, 12] = "Projection";
                    reportWorksheet.Cells[headerRow - 1, 12].Interior.Color = ColorTranslator.FromHtml("#e3e3e3");
                    reportWorksheet.Cells[headerRow - 1, 12].Font.Size = FontSizeHeader;
                    reportWorksheet.Cells[headerRow - 1, 12].Font.Bold = true;
                    reportWorksheet.Cells[headerRow - 1, 12].RowHeight = RowHeightHeader;
                    reportWorksheet.Cells[headerRow - 1, 12].Style.HorizontalAlignment = XlHAlign.xlHAlignCenter; 
                }

               


                //Autofit all cells with content.
                reportWorksheet.Columns.AutoFit();

                //Hide the columns we don't want to show: B, C, E, F
                reportWorksheet.Range["B:B"].EntireColumn.Hidden = true;
                reportWorksheet.Range["C:C"].EntireColumn.Hidden = true;
                reportWorksheet.Range["E:E"].EntireColumn.Hidden = true;
                reportWorksheet.Range["F:F"].EntireColumn.Hidden = true;

                //Add the chart
               // GenerateChart();

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
                "WIP Projected",
                "Units (Actual)",
                "UOM (Actual)",
                "% Complete",
                "Team Budget",
                "Earned Hrs",
                "Actual Hrs",
                "Performance",
                "Hours (Projected to Completion)"
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
            OverProjection = ColorTranslator.FromHtml("#ffc2c3");
            UnderProjection = ColorTranslator.FromHtml("#d0f0c0");
            //BudgetSummaryRowColor = XlRgbColor.rgbLightGray;
            //ActualSummaryRowColor = ColorTranslator.FromHtml("231,231,231"); 
        }

        /// <summary>
        /// Generates the bar chart for the Labor Reporting Chart.
        /// </summary>
        private void GenerateChart()
        {
            //const string fileName = "C:\\Book1.xlsx";
            //string topLeft = "A1";
            //string bottomRight = "A4";
            string graphTitle = "Graph Title";
            string xAxis = "Activity";
            string yAxis = "Performance";

            //Add chart
            var charts = reportWorksheet.ChartObjects() as ChartObjects;
            var chartObject = charts.Add(300, 10, 300, 300) as ChartObject;
            var barChart = chartObject.Chart;

            //Set chart range.
            var barChartPhaseCodeRange = reportWorksheet.Range[reportWorksheet.Cells[headerRow + 1, 1], reportWorksheet.Cells[5, 1]];
            var barChartPerformanceRange = reportWorksheet.Range[reportWorksheet.Cells[headerRow + 1, 10], reportWorksheet.Cells[5, 10]];
            //var barChartPercentCompleteRange = reportWorksheet.Range[reportWorksheet.Cells[headerRow + 1, 8], reportWorksheet.Cells[5, 8]];
            var barChartData = Excel.Union(barChartPhaseCodeRange, barChartPerformanceRange);

            barChart.SetSourceData(barChartData);

            //Set chart properties.
            barChart.ChartType = XlChartType.xlBarClustered;
            barChart.ChartWizard(Source: barChartData,
                Title: graphTitle,
                CategoryTitle: xAxis,
                ValueTitle: yAxis);
        }



    }
}