using Estimating.ProgressReporter.Model;
using Estimating.SQLService;
using SQLManager;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Estimating.ProgressReporter.Services
{
    public class CostCodeDataService
    {
        private string _jobNumber;
        private List<SystemEstimate> _systemEstimateList;
        private SQLService.SQLHelper _sqlControl;
        private string estimateDatabaseString;
        private string spectrumDatabaseString;

        public CostCodeDataService(string jobNumber)
        {
            _jobNumber = jobNumber;

            try
            {
                //Attempt to populate the connection strings.
                estimateDatabaseString = Properties.Settings.Default.Estimating;
                spectrumDatabaseString = Properties.Settings.Default.SPECTRUM;
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to retrieve database connection strings");
                throw;
            }
        }

        public int GetActualHoursByPhaseCode(string phaseCode)
        {
            return 343;
        }

        /// <summary>
        /// Returns the 'Earned' (estimated) hours for the provided phase code and system name.  
        /// </summary>
        /// <remarks>
        /// Connects to the "BudgetVSTO" database.
        /// </remarks>
        /// <param name="phaseCode"></param>
        /// <returns></returns>
        public int GetEarnedHours(string systemName, string phaseCode)
        {

            if (ValidateSystemName(systemName))
            {
                SQLControl sql = new SQLControl(estimateDatabaseString);
                sql.AddParam("@jobNumber", _jobNumber);
                sql.AddParam("@systemName", systemName);
                sql.AddParam("@phaseCode", phaseCode);

                sql.ExecQuery("SELECT TOP(1) EarnedHours FROM EstimateMain WHERE SystemName = @systemName AND PhaseCode = @phaseCode AND JobNumber = @jobNumber");
                //sql.ExecQuery("SELECT * FROM EstimateMain");
                if (sql.HasException())
                {
                    throw new Exception("Failure in the SQL class while retrieving Earned Hours.");
                }
                else if (sql.DBDT.Rows.Count == 0)
                {
                    //MessageBox.Show("There doesn't appear to be a record for the system: " + systemName + " in Job Number: " + _jobNumber + ". This could be:  \n" +
                    //    "1. Because of a mismatch in system names between the name contained in the estimate sheet and the name contained in the report. \n" +
                    //    "2. Because a phase code was included on the CSV report that wasn't included in the original Estimate." );
                    return 0;
                }
                else
                {
                    int earnedHours = Convert.ToInt32(sql.DBDT.Rows[0].ItemArray[0].ToString());
                    return earnedHours;
                } 
            }
            else
            {
                return 0;
            }
        }


        public int GetBudgetedHoursByPhaseCode(string phaseCode)
        {
            return 19;
        }

        /// <summary>
        /// Validates the provided system name by matching it with a corresponding entry in the 'EstimateMain' table.
        /// </summary>
        /// <param name="systemName"></param>
        /// <returns></returns>
        public bool ValidateSystemName(string systemName)
        {
            SQLControl sql = new SQLControl(estimateDatabaseString);
            sql.AddParam("@jobNumber", _jobNumber);
            sql.AddParam("@systemName", systemName);
            sql.ExecQuery("SELECT * FROM EstimateMain WHERE SystemName = @systemName AND JobNumber = @jobNumber");
            if (sql.HasException())
            {
                throw new Exception(sql.Exception.ToString());
            }
            else if(sql.DBDT.Rows.Count == 0)
            {
                MessageBox.Show("The system: " + systemName + " was not found in the Estimate record.  This could be due to a mismatch between the case/spelling used in " +
                    "CSV report file.  Check the Estimate sheet for the job and make sure the system names match exactly.  If they do not, alter the CSV file entry and try again");
                return false;
            }
            else
            {
                return true;
            }
        }


    }
}
