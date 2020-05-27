using Estimating.ProgressReporter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estimating.ProgressReporter.Services
{
    public class ReportDataService : IReportDataService
    {
        private List<SystemReport> _reportedSystemsList;

        public ReportDataService(List<SystemReport> reportedSystemsList)
        {
            _reportedSystemsList = reportedSystemsList;
        }

        /// <summary>
        /// Returns the list of SystemReport objects that was provided when the class was instanced.
        /// </summary>
        /// <returns></returns>
        public List<SystemReport> GetReportedSystems()
        {
            return _reportedSystemsList;
        }
    }
}
