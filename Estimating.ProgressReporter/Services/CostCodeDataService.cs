using Estimating.ProgressReporter.Model;
using Estimating.SQLService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estimating.ProgressReporter.Services
{
    public class CostCodeDataService
    {
        private string _jobNumber;
        private List<SystemEstimate> _systemEstimateList;
        private SQLControl _sqlControl;

        public CostCodeDataService(string jobNumber)
        {
            _jobNumber = jobNumber;
        }


        public int GetEstimatedHoursByPhaseCode(string phaseCode)
        {
            return 5;
        }

        public int GetBudgetedHoursByPhaseCode(string phaseCode)
        {
            return 20;
        }
    }
}
