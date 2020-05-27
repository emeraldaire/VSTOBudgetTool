using Estimating.ProgressReporter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estimating.ProgressReporter.Services
{
    public interface IModelReportingService
    {
        ComparatorReport GetCompleteModelReport();
        List<ComparatorResult> GetFinishedSystems();
        List<ComparatorResult> GetPartiallyFinishedSystems();
        List<ComparatorResult> GetReportedSystems();
        List<SystemEstimate> GetUnreportedSystems();
    }
}
