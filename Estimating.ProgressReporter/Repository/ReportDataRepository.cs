using Estimating.ProgressReporter.Interfaces.Model;
using Estimating.ProgressReporter.Model;
using Estimating.ProgressReporter.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Estimating.ProgressReporter.Repository
{
    /// <summary>
    /// Holds the ReportModel object, which contains the list of systems being reported from the field report. 
    /// </summary>
    public class ReportDataRepository : IReportDataRepository
    {
        public IReportDataService _reportDataService { get; set; }
       
        public ReportModel ReportModel { get; set; }

        public string _jobNumber { get; set; }

        public ReportDataRepository(string jobNumber, IReportDataService reportDataService)
        {
            _jobNumber = jobNumber;
            _reportDataService = reportDataService;
        }

        public void LoadReportModel()
        {
            if (!String.IsNullOrEmpty(_jobNumber))
            {
                //MessageBox.Show($"Retrieving System Report Model for Job: {_jobNumber}");
                //Get the list of system estimates from the data service, then use the list to populate the EstimateModel.
                ReportModel = new ReportModel(_jobNumber, _reportDataService.GetReportedSystems());
            }
            else
            {
                throw new Exception("The job number failed to be assigned to the ReportDataRepository.  Please check the parameters that were provided by the caller when this class was instanced.");
            }
        }
    }
}
