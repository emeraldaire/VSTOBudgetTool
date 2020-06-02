using Estimating.ProgressReporter.Errors;
using Estimating.ProgressReporter.Interfaces.Model;
using Estimating.ProgressReporter.Interfaces.Services;
using Estimating.ProgressReporter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Estimating.ProgressReporter.Services
{
    public class ModelReportingService : IModelReportingService
    {
        private EstimateModel _estimateModel { get; set; }
        private ReportModel _reportModel { get; set; }

        private ComparatorReport ModelReport { get; set; }
        private CostCodeReport CostCodeReport { get; set; }

        //CONSTRUCTOR
        public static ModelReportingService LoadModelReportingService(EstimateModel estimateModel, ReportModel reportModel)
        {
            ModelReportingService modelReportingService = new ModelReportingService();

            modelReportingService._reportModel = reportModel;
            modelReportingService._estimateModel = estimateModel;
            try
            { modelReportingService.GenerateModelReport(); }
            catch(Exception e)
            { Console.WriteLine(e.Message); }

            return modelReportingService;

        }

        public ModelReportingService(ReportModel reportModel)
        {
            _reportModel = reportModel;
        }
        public ModelReportingService()
        {
        }

        //public ModelReportingService(EstimateModel estimateModel, ReportModel reportModel)
       
            
        /// <summary>
        /// Returns the completed report object or a NULL value if the report cannot be completed.
        /// </summary>
        /// <returns></returns>
        private void GenerateModelReport()
        {
            try
            {   
                ComparatorReport comparatorReport = new ComparatorReport(_estimateModel, _reportModel);
                comparatorReport.GenerateSystemReport();
                ModelReport = comparatorReport;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jobNumber"></param>
        public void GenerateCostCodeReport(string jobNumber)
        {
            try
            {
                CostCodeReport costCodeReport = new CostCodeReport(jobNumber, _reportModel);
                costCodeReport.GenerateCostCodeReport();
                CostCodeReport = costCodeReport;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Returns a list of ComparatorResult objects whose "IsWorkCompleted" property is TRUE.
        /// </summary>
        /// <returns></returns>
        public List<ComparatorResult> GetFinishedSystems()
        {
            if (ModelReport != null)
            {
                List<ComparatorResult> finishedSystems = new List<ComparatorResult>();
                foreach (ComparatorResult c in ModelReport.ReportedSystems)
                {
                    if (c.IsWorkCompleted == true)
                    {
                        finishedSystems.Add(c);
                    }
                }
                return finishedSystems;
            }
            else
            {
                throw new ModelReportWasEmptyException();
            }
        }

        /// <summary>
        /// Returns a list of ComparatorResult objects for systems that have been reported on, but remain unfinished.
        /// </summary>
        /// <returns></returns>
        public List<ComparatorResult> GetPartiallyFinishedSystems()
        {
            if (ModelReport != null)
            {
                List<ComparatorResult> partiallyFinishedSystems = new List<ComparatorResult>();
                foreach (ComparatorResult c in ModelReport.ReportedSystems)
                {
                    if (c.IsWorkCompleted == false)
                    {
                        partiallyFinishedSystems.Add(c);
                    }
                }

                return partiallyFinishedSystems;
            }
            else
            {
                throw new ModelReportWasEmptyException();
            }
        }

        /// <summary>
        /// Returns a list of ComparatorResult objects for all systems that were reported on. 
        /// </summary>
        /// <returns></returns>
        public List<ComparatorResult> GetReportedSystems()
        {
            if (ModelReport != null)
            {
                List<ComparatorResult> reportedSystems = new List<ComparatorResult>();
                foreach (ComparatorResult c in ModelReport.ReportedSystems)
                {
                        reportedSystems.Add(c);
                }

                return reportedSystems;
            }
            else
            {
                throw new ModelReportWasEmptyException();
            }
        }

        /// <summary>
        /// Returns a list of SystemEstimate objects for systems that were not included in the field report.  Naturally, all of these systems are 
        /// are reported as unfinished.
        /// </summary>
        /// <returns></returns>
        public List<SystemEstimate> GetUnreportedSystems()
        {
            if (ModelReport != null)
            {
                List<SystemEstimate> partiallyFinishedSystems = ModelReport.UnreportedSystems;

                return partiallyFinishedSystems;
            }
            else
            {
                throw new ModelReportWasEmptyException();
            }
        }

        /// <summary>
        /// Returns the completed ModelReport if it exists; otherwise, returns null.
        /// </summary>
        /// <returns></returns>
        public ComparatorReport GetCompleteModelReport()
        {
            if(ModelReport != null )
            {
                return ModelReport;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns the completed CostCodeReport if it has been populated; otherwise, returns null.
        /// </summary>
        /// <returns></returns>
        public CostCodeReport GetCompleteCostCodeReport()
        {
            if (CostCodeReport != null)
            {
                return CostCodeReport;
            }
            else
            {
                return null;
            }
        }


    }
}
