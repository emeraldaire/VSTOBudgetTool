using Estimating.ProgressReporter.Model;
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
        private string estimateDatabaseString;
        private string spectrumDatabaseString;

        public CostCodeDataService(string jobNumber)
        {
            _jobNumber = jobNumber;

            try
            {
                //Attempt to populate the connection strings.
                estimateDatabaseString = ConnectionStringService.GetConnectionString("Estimate");
                spectrumDatabaseString = ConnectionStringService.GetConnectionString("Spectrum");
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to retrieve database connection strings");
                throw;
            }
        }

        /// <summary>
        /// Returns the summed 'Total_Hours' from [JC_TRANSACTION_HISTORY_MC] by Phase Code, Job Number, and (redundant) Cost Type.
        /// </summary>
        /// <param name="phaseCode"></param>
        /// <returns></returns>
        public double GetActualHoursByPhaseCode(string phaseCode)
        {
            phaseCode = phaseCode.Remove(4, 1);
           
            SQLControl sql = new SQLControl(spectrumDatabaseString);
            //sql.AddParam("@jobNumber", _jobNumber);
            sql.AddParam("@jobNumber", "   " + _jobNumber);
            sql.AddParam("@phaseCode", phaseCode);
            sql.AddParam("@costType", "L");
            //sql.AddParam("@costType", "L");
            sql.ExecQuery("SELECT SUM(Total_Hours) FROM JC_TRANSACTION_HISTORY_MC WHERE Job_Number = @jobNumber AND Cost_Type = @costType AND Phase_Code = @phaseCode");

            //sql.ExecQuery("SELECT * FROM EstimateMain");
            if (sql.HasException())
            {
                throw new Exception("Failure in the SQL class while retrieving Earned Hours.");
            }
            else if (sql.DBDT.Rows.Count == 0 || sql.DBDT == null)
            {
                //MessageBox.Show("There doesn't appear to be a record for the system: " + systemName + " in Job Number: " + _jobNumber + ". This could be:  \n" +
                //    "1. Because of a mismatch in system names between the name contained in the estimate sheet and the name contained in the report. \n" +
                //    "2. Because a phase code was included on the CSV report that wasn't included in the original Estimate." );
                return 0;
            }
            else
            {
                //int budgetedHours = Convert.ToInt32(sql.DBDT.Rows[0].ItemArray[0].ToString());
                double actualHours = Convert.ToDouble(sql.DBDT.Rows[0].ItemArray[0].ToString());
                return actualHours;
            }
        }

        /// <summary>
        /// Returns the 'Earned' (estimated) hours from [EstimateMain] for the provided phase code and system name.  
        /// </summary>
        /// <remarks>
        /// Connects to the "BudgetVSTO" database.
        /// </remarks>
        /// <param name="phaseCode"></param>
        /// <returns></returns>
        public double GetEarnedHours(string systemName, string phaseCode)
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
                    double earnedHours = Convert.ToDouble(sql.DBDT.Rows[0].ItemArray[0].ToString());
                    return earnedHours;
                } 
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Returns the 'Original_Est_Hours' from [JC_PHASE_MASTER_MC] for the provided Job Number and Phase Code, where Cost Type is always "L" for "Labor"
        /// </summary>
        /// <param name="phaseCode"></param>
        /// <returns></returns>

        public double GetBudgetedHoursByPhaseCode(string phaseCode)
        {
            phaseCode = phaseCode.Remove(4, 1);
            SQLControl sql = new SQLControl(spectrumDatabaseString);
            //sql.AddParam("@jobNumber", _jobNumber);
            sql.AddParam("@jobNumber", "   " + _jobNumber);
            sql.AddParam("@phaseCode", phaseCode);
            sql.AddParam("@costType", "L");

            sql.ExecQuery("SELECT TOP(1) Original_Est_Hours FROM JC_PHASE_MASTER_MC WHERE Phase_Code = @phaseCode AND Job_Number = @jobNumber AND Cost_Type = @costType");

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
                //int budgetedHours = Convert.ToInt32(sql.DBDT.Rows[0].ItemArray[0].ToString());
                double budgetedHours = Convert.ToDouble(sql.DBDT.Rows[0].ItemArray[0].ToString());
                return budgetedHours;
            }
        }


        /// <summary>
        /// Returns the "Projected_Hours" field from the "JC_PROJ_COST_HISTORY_MC" table in SPECTRUM.
        /// </summary>
        /// <param name="phaseCode"></param>
        /// <returns></returns>
        public double GetProjectedHoursByPhaseCode(string phaseCode)
        {
            phaseCode = phaseCode.Remove(4, 1);
            SQLControl sql = new SQLControl(spectrumDatabaseString);
            //sql.AddParam("@jobNumber", _jobNumber);
            sql.AddParam("@jobNumber", "   " + _jobNumber);
            sql.AddParam("@phaseCode", phaseCode);
            sql.AddParam("@costType", "L");
            
            sql.ExecQuery("SELECT SUM(Projected_Hours) FROM JC_PROJ_COST_HISTORY_MC WHERE Phase = @phaseCode AND Job = @jobNumber AND Cost_Type = @costType");

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
                //int budgetedHours = Convert.ToInt32(sql.DBDT.Rows[0].ItemArray[0].ToString());
                double projectedHours = Convert.ToDouble(sql.DBDT.Rows[0].ItemArray[0].ToString());
                return projectedHours;
            }
        }

        /// <summary>
        /// Returns the summed earnedhours from EstimateMain for provided jobnumber and phase code
        /// </summary>
        /// <param name="phaseCode"></param>
        /// <returns></returns>
        public double GetTeamBudgetHours(string phaseCode)
        {

            if (ValidatePhaseCode(phaseCode))
            {
                SQLControl sql = new SQLControl(estimateDatabaseString);
                //sql.AddParam("@jobNumber", _jobNumber);
                sql.AddParam("@jobNumber", _jobNumber);
                sql.AddParam("@phaseCode", phaseCode);
                //sql.AddParam("@costType", "L");
                sql.ExecQuery("SELECT SUM(EarnedHours) FROM EstimateMain WHERE JobNumber = @jobNumber AND PhaseCode = @phaseCode");

                //sql.ExecQuery("SELECT * FROM EstimateMain");
                if (sql.HasException())
                {
                    throw new Exception("Failure in the SQL class while retrieving Earned Hours.");
                }
                else if (sql.DBDT.Rows.Count == 0 || sql.DBDT == null)
                {
                    //MessageBox.Show("There doesn't appear to be a record for the system: " + systemName + " in Job Number: " + _jobNumber + ". This could be:  \n" +
                    //    "1. Because of a mismatch in system names between the name contained in the estimate sheet and the name contained in the report. \n" +
                    //    "2. Because a phase code was included on the CSV report that wasn't included in the original Estimate." );
                    return 0;
                }
                else
                {
                    //int budgetedHours = Convert.ToInt32(sql.DBDT.Rows[0].ItemArray[0].ToString());
                    //int teamBudgetHours = Convert.ToInt32(sql.DBDT.Rows[0].ItemArray[0].ToString());
                    double teamBudgetHours = Convert.ToDouble(sql.DBDT.Rows[0].ItemArray[0].ToString());
                    return teamBudgetHours;
                } 
            }
            else
            {
                return 0;
            }


        }

        private bool ValidatePhaseCode(string phaseCode)
        {
            SQLControl sql = new SQLControl(estimateDatabaseString);
            sql.AddParam("@jobNumber", _jobNumber);
            sql.AddParam("@phaseCode", phaseCode);
            sql.ExecQuery("SELECT * FROM EstimateMain WHERE JobNumber = @jobNumber AND PhaseCode = @phaseCode");
            if (sql.HasException())
            {
                throw new Exception(sql.Exception.ToString());
            }
            else if (sql.DBDT.Rows.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }

        }


        /// <summary>
        /// Validates the provided system name by matching it with a corresponding entry in the 'EstimateMain' table.
        /// </summary>
        /// <param name="systemName"></param>
        /// <returns></returns>
        public bool ValidateSystemName(string systemName)
        {
            //To avoid displaying multiple error messages for the same system, a list of systems for which errors have been displayed is kept.  When the next error
            //is triggered, the message will only display if the system is not found in this list. 
            List<string> uniqueSystemsList = new List<string>();

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
                if (uniqueSystemsList.Contains(systemName))
                {
                    //Do nothing.
                }
                else
                {
                    MessageBox.Show("The system: " + systemName + " was not found in the Estimate record.  This could be due to a mismatch between the case/spelling used in " +
                        "CSV report file.  Check the Estimate sheet for the job and make sure the system names match exactly.  If they do not, alter the CSV file entry and try again");

                    //Add the system name to the uniqueSystemsList so that no more error messages appear for this system.
                    uniqueSystemsList.Add(systemName);
                }
                return false;
            }
            else
            {
                return true;
            }
        }

    }
}
