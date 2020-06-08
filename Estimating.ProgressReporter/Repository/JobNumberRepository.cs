using Estimating.SQLService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estimating.ProgressReporter.Repository
{
    public class JobNumberRepository
    {
        private SQLService.SQLHelper sqlControl { get; set; }


        public List<string> GetAll()
        {
            List<string> activeEstimateJobNumbers = new List<string>();

            activeEstimateJobNumbers.Add("2170507");
            activeEstimateJobNumbers.Add("2181105");
            activeEstimateJobNumbers.Add("2180608");
            activeEstimateJobNumbers.Add("2151107");
            activeEstimateJobNumbers.Add("2190204");
            activeEstimateJobNumbers.Add("2180416");
            activeEstimateJobNumbers.Add("2180410");
            activeEstimateJobNumbers.Add("2160709");

            return activeEstimateJobNumbers;
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
