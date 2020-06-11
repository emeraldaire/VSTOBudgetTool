using System;
using System.Globalization;
using Estimating.ProgressReporter.Model;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using SQLManager;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            //Convert the systemEstimateLIst into EstimateTransaction form, ready for database commit.
            List<EstimateTransaction> transactionData = ConvertToEstimateTransaction(systemEstimateList);

            if (transactionData != null && transactionData.Count != 0)
            {
                try
                {
                    //Insert a new header entry into the database.
                    SQLControl sql = new SQLControl(ConnectionStringService.GetConnectionString("Estimate"));
                    sql.AddParam("@jobNumber", _jobNumber);
                    sql.AddOutputParam("@estimateID", SqlDbType.Int, 4);
                    int nextEstimateID = (int)sql.GetReturnValueFromStoredProcedure("spInsertEstimateHeader", "@estimateID");
                   
                    //If the new estimate header was successfully added, continue with the process.
                    if (nextEstimateID > 0)
                    {
                        //Assign the estimate ID to the transaction objects.
                        //int newEstimateID = 12;
                        foreach (EstimateTransaction t in transactionData)
                        {
                            t.EstimateID = nextEstimateID;
                        }

                        //Send the transaction list to be inserted in the database.
                        InsertMultipleRecords(transactionData);
                    }

                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message.ToString());
                    throw;
                } 
            }
            else
            {
                throw new Exception("Estimate commit aborted because transaction data list failed to populate correctly.");
            }

        }

        /// <summary>
        /// Insert multiple records into the database transaction.
        /// </summary>
        /// <param name="estimateList"></param>
        private void InsertMultipleRecords(IEnumerable<EstimateTransaction> transactionList)
        {
            //Create a datatable from the list.
            var estimateRecords = ConvertToDataTable(transactionList);

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
                            connection.Close();

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

        /// <summary>
        /// Converts the SystemEstimate list into a list of EstimateTransaction objects; all properties are assigned here except for the 
        /// new estimate ID that will go with the database INSERT operation.  The caller will provide and assign this value.
        /// </summary>
        /// <param name="systemEstimateList"></param>
        /// <param name="newEstimateID"></param>
        /// <returns></returns>
        private List<EstimateTransaction> ConvertToEstimateTransaction(List<SystemEstimate> systemEstimateList)
        {
            //Set the date 
            DateTime commitDate = DateTime.Now;

            List<EstimateTransaction> estimateTransactionList = new List<EstimateTransaction>();

            //Unpack the system estimate by phase code; create an instance of an EstimateTransaction for each phase code, then add to the estimateTransactionList.
            foreach(SystemEstimate s in systemEstimateList)
            {
                foreach(PhaseCode p in s.PhaseCodes)
                {
                    EstimateTransaction estimateTransaction = new EstimateTransaction()
                    {
                        //EstimateID = newEstimateID,
                        EstimateCommitDate = commitDate,
                        JobNumber = _jobNumber,
                        SystemName = s.Name,
                        FullPhaseCode = p.FullPhaseCode,
                        EarnedHours = p.EarnedHours
                    };

                    //Add the newly created obect to the list.
                    estimateTransactionList.Add(estimateTransaction);
                }
            }

            return estimateTransactionList;

        }





    }
}
