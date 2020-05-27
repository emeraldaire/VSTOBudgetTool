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
    /// Holds the EstimateModel object, which contains a list of system estimate objects for the job number associated with the field report.
    /// </summary>
    public class ModelDataRepository : IModelDataRepository
    {
        public EstimateModel EstimateModel { get; set; }

        public string _jobNumber { get; set; }

        public IEstimateDataService _estimateDataService { get; set; }

        public ModelDataRepository(string jobNumber, IEstimateDataService estimateDataService)
        {
            _jobNumber = jobNumber;
            _estimateDataService = estimateDataService;
        }

        public void LoadEstimateModel()
        {
            if(!String.IsNullOrEmpty(_jobNumber))
            {
                //MessageBox.Show($"Retrieving Estimate Model for Job: {_jobNumber}");
                //Get the list of system estimates from the data service, then use the list to populate the EstimateModel.
                EstimateModel = new EstimateModel(_jobNumber, _estimateDataService.GetSystemEstimatesByJobNumber(_jobNumber));
            }
            else
            {
                throw new Exception("The job number failed to be assigned to the ModelDataRepository.  Please check the parameters that were provided by the caller when this class was instanced.");
            }
        }
    }
}
