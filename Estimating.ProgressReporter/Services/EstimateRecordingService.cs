using System;
using Estimating.SQLService;
using Estimating.ProgressReporter.Model;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estimating.ProgressReporter.Services
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// DATE: 6/2/20
    /// AUTHOR: Noah B
    /// This class uses data provided by the VSTO to record SystemEstimate objects to the database.  There should be no implementation of 
    /// Microsoft.Office.Interop.Excel in this class.  The VSTO is responsible for handling spreadsheet data and generating the SystemEstimate list to 
    /// be consumed by this class.
    /// </remarks>
    public class EstimateRecordingService
    {
        private string _jobNumber;

        public string JobNumber
        {
            get { return _jobNumber; }
            set { _jobNumber = value; }
        }

        public EstimateRecordingService(string jobNumber)
        {
            JobNumber = jobNumber; 
        }

        /// <summary>
        /// Commits the SystemEstimate objects contained in the provided list to the database.
        /// </summary>
        /// <param name="systemEstimateList"></param>
        public void Commit(List<SystemEstimate> systemEstimateList)
        {

        }

        /// <summary>
        /// Insert multiple records into the database transaction.
        /// </summary>
        /// <param name="estimateList"></param>
        private void InsertMultipleRecords(IEnumerable<EstimateTransaction> estimateList)
        {
            //Create a datatable from the list.
            var estimateRecords = ConvertToDataTable(estimateList);

            //insert in DB
            using (var connection = ConnectionStringService.GetConnection("Estimate"))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
                    {
                        try
                        {
                            bulkCopy.DestinationTableName = "EstimateMain";
                            bulkCopy.WriteToServer(estimateRecords);
                            transaction.Commit();

                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            connection.Close();
                        }

                    }
                }
            }



        }

        /// <summary>
        /// Converts the provided list of 'EstimateTransaction' objects to a DataTable.
        /// </summary>
        /// <param name="estimateList"></param>
        /// <returns></returns>
        private DataTable ConvertToDataTable(IEnumerable<EstimateTransaction> estimateList)
        {
            var table = new DataTable();
            table.Columns.Add("EstimateID", typeof(int));
            table.Columns.Add("JobNumber", typeof(string));
            table.Columns.Add("SystemName", typeof(string));
            table.Columns.Add("PhaseCode", typeof(string));
            table.Columns.Add("EarnedHours", typeof(int));

            foreach (EstimateTransaction est in estimateList)
            {
                table.Rows.Add(new object[]
                {
                est.EstimateID,
                est.JobNumber,
                est.SystemName,
                est.FullPhaseCode,
                est.EarnedHours
                });
            }

            return table;
        }







    }
}
