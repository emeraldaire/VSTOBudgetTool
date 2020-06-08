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
        private SQLService.SQLHelper _sqlControl;

        public CostCodeDataService(string jobNumber)
        {
            _jobNumber = jobNumber;
        }

        public int GetActualHoursBySystemName(string systemName)
        {
            switch (systemName)
            {
                case "FC-1":
                    return 2;
                case "FC-2":
                    return 3;
                case "FC-3":
                    return 7;
                default:
                    return 5;
            }
        }

        public int GetEstimatedHoursByPhaseCode(string phaseCode)
        {
            switch (phaseCode)
            {
                case "0001-0401":
                    return 2;
                case "0001-0601":
                    return 3;
                default:
                    return 5;
            }
        }

        public int GetBudgetedHoursByPhaseCode(string phaseCode)
        {
            switch (phaseCode)
            {
                case "0001-0401":
                    return 59;
                case "0001-0601":
                    return 27;
                default:
                    return 50;
            }
        }
    }
}
