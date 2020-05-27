using Estimating.ProgressReporter.Interfaces.Model;
using Estimating.ProgressReporter.Interfaces.Services;
using Estimating.ProgressReporter.Model;
using Estimating.ProgressReporter.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estimating.ProgressReporter.Services
{
    /// <summary>
    /// Primary service class for client.  This class oversees creation of repositories and report processing; its primary purpose is to generate and return a 
    /// queryable report object to the caller. 
    /// </summary>
    /// <remarks>
    /// DATE: 5/21/20
    /// AUTHOR: Noah B
    /// ABSTRACT: The primary inputs for this class are the list of the reported systems and the job number for the report.  By requiring the List (instead of the 
    /// filepath to the report), the dependency  on the "CSVHelper" is removed and the duties for validating the file have been shifted to the caller.  
    /// The primary deliverable of this class is a queryable 'ComparatorReport' object which the caller can use to pipe and display the data.  
    /// </remarks>
    public class ClientReportService 
    {
        private string _jobNumber { get; set; } 
        private IDataRepository _dataRepository { get; set; }
        private IModelReportingService _modelReportingService { get; set; }

        public ClientReportService(string jobNumber)
        {
            _jobNumber = jobNumber;
            //_dataRepository = dataRepository;
            //_modelReportingService = modelReportingService;
        }

        public ComparatorReport GetReportSummary(List<SystemReport> reportedSystemsList)
        {
            //Generate the data repository, which contains the Model and Report repositories, along with the ReportModel and EstimateModel.
            _dataRepository = DataRepository.LoadDataRepository(_jobNumber, reportedSystemsList);
            //Run the report; it is processed by default when the ModelReportingService is instantiated.
            _modelReportingService = ModelReportingService.LoadModelReportingService(_dataRepository.EstimateModel, _dataRepository.ReportModel);

            return _modelReportingService.GetCompleteModelReport(); ;
        }
    }
}
