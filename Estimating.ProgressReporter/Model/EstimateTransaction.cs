using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estimating.ProgressReporter.Model
{
    /// <summary>
    /// This is a helper class which serves as a temporary container for SystemEstimate data about to be committed to the database.
    /// A list of EstimateTransaction objects is sent to the 'Commit' method of the EstimateRecordingService. 
    /// </summary>
    public class EstimateTransaction
    {
        public int EstimateID { get; set; }
        public DateTime EstimateCommitDate { get; set; }
        public string JobNumber { get; set; }
        public string SystemName { get; set; }
        public string FullPhaseCode { get; set; }
        public double EarnedHours { get; set; }


    }
}
