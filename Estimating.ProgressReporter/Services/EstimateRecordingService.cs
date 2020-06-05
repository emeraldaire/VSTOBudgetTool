using Estimating.ProgressReporter.Model;
using System;
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
    /// This class uses data provided by the VSTO to generate and record EstimateModel objects to the database.  There should be no implementation of 
    /// Microsoft.Office.Interop.Excel in this class; VSTO will be responsible for that. 
    /// </remarks>
    public class EstimateRecordingService
    {
        /// <summary>
        /// Returns a single SystemEstimate object to the caller.  Used by VSTO to instantiate objects that will be added to a List. 
        /// </summary>
        /// <returns></returns>
        public SystemEstimate GenerateSystemEstimate()
        {
            return null;
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
