﻿using Estimating.ProgressReporter.Services;
using SQLManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estimating.ProgressReporter.Repository
{
    public class JobNumberRepository
    {
        private string estimateDatabaseString = ConnectionStringService.GetConnectionString("Estimate");

        public List<string> GetAll()
        {
            SQLControl sql = new SQLControl(estimateDatabaseString);
            sql.ExecQuery("SELECT JobNumber FROM EstimateHeader");

            if (sql.HasException())
            {
                throw new Exception("The JobNumberRepository was unable to populate the list of job numbers");
            }
            else if (sql.DBDT.Rows.Count == 0)
            {
                //MessageBox.Show("There doesn't appear to be a record for the system: " + systemName + " in Job Number: " + _jobNumber + ". This could be:  \n" +
                //    "1. Because of a mismatch in system names between the name contained in the estimate sheet and the name contained in the report. \n" +
                //    "2. Because a phase code was included on the CSV report that wasn't included in the original Estimate." );
                return null;
            }
            else
            {
                List<string> jobNumberList = new List<string>();

                for(int i = 0; i < sql.DBDT.Rows.Count; i++)
                {
                    string jobNumber = sql.DBDT.Rows[i].ItemArray[0].ToString();
                    jobNumberList.Add(jobNumber);
                }

                return jobNumberList;
            }
        }

        /// <summary>
        /// Returns true if the job number is found in the estimating database.  Only one estimate record is allowed per job number.
        /// </summary>
        /// <param name="jobNumber"></param>
        /// <returns></returns>
        public bool IsDuplicateJobNumber(string jobNumber)
        {
            using (SQLControl sql = new SQLControl(ConnectionStringService.GetConnectionString("Estimate")))
            {
                sql.AddParam("@jobNumber", jobNumber);
                sql.ExecQuery("SELECT * FROM EstimateHeader WHERE JobNumber = @jobNumber");
                
                if(sql.HasException())
                {
                    throw new Exception("Error in SQLControl class while attempting to detect duplicate job numbers.");
                }

                if(sql.DBDT.Rows.Count == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }

            };

        }


        public string GetJobNumberByName(string jobName)
        {
            return null;
        }

        public void Update(string jobNumber)
        {

        }

        public void Delete(string jobNumber)
        {
            //Mark estimate for job number as 'inactive'; action initiated by privileged user.
        }
    }
}
