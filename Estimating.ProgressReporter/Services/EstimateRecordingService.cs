using System;
using Estimating.SQLService;
using Estimating.ProgressReporter.Model;
using System.Collections.Generic;
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


    }
}
