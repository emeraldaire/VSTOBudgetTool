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
    public class DataRepository : IDataRepository
    {
        private List<SystemReport> _reportedSystemList { get; set; }
        public string JobNumber { get; set; }
        private IModelDataRepository ModelDataRepository { get; set; }
        private IReportDataRepository ReportDataRepository { get; set; }
        public EstimateModel EstimateModel { get; set; }
        public ReportModel ReportModel { get; set; }

        public DataRepository(string jobNumber, List<SystemReport> reportedSystemList)
        {
            JobNumber = jobNumber;
            _reportedSystemList = reportedSystemList;
        }

        public static DataRepository LoadDataRepository(string jobNumber, List<SystemReport> reportedSystemList)
        {
            DataRepository dataRepository = new DataRepository(jobNumber, reportedSystemList);
            dataRepository.LoadRepositories();
            return dataRepository;
        }

        private void LoadEstimateModel()
        {
            //Create a new estimate model repository.  Model Repository requires IEstimateDataService
            ModelDataRepository = new ModelDataRepository(JobNumber, new EstimateDataService());
            ModelDataRepository.LoadEstimateModel();
            //Assign the result to the local instance.
            EstimateModel = ModelDataRepository.EstimateModel;   
        }

        private void LoadReportModel(List<SystemReport> reportedSystemList)
        {
            //Create a new report model repository.
            ReportDataRepository = new ReportDataRepository(JobNumber, new ReportDataService(reportedSystemList));
            ReportDataRepository.LoadReportModel();
            ReportDataRepository._jobNumber = JobNumber;
            //Assign the result to the local instance.

            ReportModel = ReportDataRepository.ReportModel;
        }

        public void LoadRepositories()
        {
            LoadEstimateModel();
            LoadReportModel(_reportedSystemList);
        }
    }
}
