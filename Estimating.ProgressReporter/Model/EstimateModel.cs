using Estimating.ProgressReporter.Interfaces.Model;
using Estimating.ProgressReporter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estimating.ProgressReporter.Model
{
    public class EstimateModel : ISystemModel<SystemEstimate>
    {
        public string JobNumber { get; set; }
        public string JobName { get; set; }
        public List<SystemEstimate> Systems { get; set; }
       
        //Constructors
        /// <summary>
        /// Generate an Estimate Model with an empty system list.
        /// </summary>
        /// <param name="jobNumber"></param>
        public EstimateModel(string jobNumber)
        {
            JobNumber = jobNumber;
            //Initialize the Systems list so that it isn't NULL and so that objects can be added by the caller.
            Systems = new List<SystemEstimate>();
        }

        /// <summary>
        /// Generate an Estimate Model with a populated system list.
        /// </summary>
        /// <param name="jobNumber"></param>
        /// <param name="systemEstimates"></param>
        public EstimateModel(string jobNumber, List<SystemEstimate> systemEstimates)
        {
            JobNumber = jobNumber;
            Systems = systemEstimates;
        }
    }
}
