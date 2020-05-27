using Estimating.ProgressReporter.Interfaces.Model;
using Estimating.ProgressReporter.Model;
using Estimating.ProgressReporter.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estimating.ProgressReporter.Repository
{
    public interface IReportDataRepository
    {
        IReportDataService _reportDataService { get; set; }
        ReportModel ReportModel { get; }
        string _jobNumber { get; set; }

        void LoadReportModel();
    }
}
