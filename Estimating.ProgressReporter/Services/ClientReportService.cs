using Estimating.ProgressReporter.Interfaces.Model;
using Estimating.ProgressReporter.Interfaces.Services;
using Estimating.ProgressReporter.Model;
using Estimating.ProgressReporter.Repository;
using SOM.BudgetVSTO.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estimating.ProgressReporter.Services
{
    /// <summary>
    /// Primary service class for client.  This class oversees creation of repositories and report processing; its primary purpose is to generate and return 
    /// queryable report objects to the caller. 
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
        private BudgetDataProvider _dataProvider { get; set; }
        private IDataRepository _dataRepository { get; set; }
        private IModelReportingService _modelReportingService { get; set; }

        public ClientReportService(string jobNumber, BudgetDataProvider dataProvider)
        {
            _jobNumber = jobNumber;
            _dataProvider = dataProvider; 
            //_dataRepository = dataRepository;
            //_modelReportingService = modelReportingService;
        }

        /// <summary>
        /// Comparator Reports provide a comparative analysis of equipment systems for a given job.  Where the 'CostCodeReport' assesses job completeness by 
        /// referencing phase codes, the Comparator Report assesses the completeness in terms of inidividual equipment systems, broken into their constituent 
        /// phase codes.
        /// </summary>
        /// <param name="reportedSystemsList"></param>
        /// <returns></returns>
        public ComparatorReport GetReportSummary(List<SystemReport> reportedSystemsList)
        {
            //Generate the data repository, which contains the Model and Report repositories, along with the ReportModel and EstimateModel.
            _dataRepository = DataRepository.LoadDataRepository(_jobNumber, reportedSystemsList);
            //Run the report; it is processed by default when the ModelReportingService is instantiated.
            _modelReportingService = ModelReportingService.LoadModelReportingService(_dataRepository.EstimateModel, _dataRepository.ReportModel, _dataProvider);

            return _modelReportingService.GetCompleteModelReport(); ;
        }

        /// <summary>
        /// Returns a completed CostCodeReport for the systems and job being reported.
        /// </summary>
        /// <param name="reportedSystemsList"></param>
        /// <returns></returns>
        public CostCodeReport GetCostCodeReportSummary(List<SystemReport> reportedSystemsList)
        {
            //Generate the data repository, which contains the Model and Report repositories, along with the ReportModel and EstimateModel.
            _dataRepository = DataRepository.LoadDataRepository(_jobNumber, reportedSystemsList);

            //Populate the ModelReportingService with the ReportModel, which is the only requirement for the CostCodeReport.
            ModelReportingService modelReportingService = new ModelReportingService(_dataRepository.ReportModel, _dataProvider);
            modelReportingService.GenerateCostCodeReport(_dataRepository.JobNumber);
            return modelReportingService.GetCompleteCostCodeReport();
        }
    }
}
