using Estimating.ProgressReporter.Enums;
using Estimating.ProgressReporter.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace Estimating.ProgressReporter.Model
{


    public class CostCodeReport
    {
        private string _jobNumber;
        private CostCodeDataService _costCodeDataService;
        private ReportModel _reportModel;
       // private Dictionary<string, SystemReport> _reportedSystemsDictionary;

        //Holds a list of comparator results for systems that were reported on in the weekly report. 
        public List<ComparatorResult> ReportedSystems;
        //If a system wasn't included in the weekly report, it will be added to a list of 'UnreportedSystems' to be included in the final report. 
        public List<SystemEstimate> UnreportedSystems;
        //Holds a CostCodeResult for each cost code being reported.
        public List<CostCodeResult> CostCodeResults;
        

        //CONSTRUCTOR
        public CostCodeReport(string jobNumber, ReportModel ReportModel)
        {
            _jobNumber = jobNumber;
            _reportModel = ReportModel;
           // _reportedSystemsDictionary = _reportModel.Systems.ToDictionary(p => p.Name, p => p);
        }

        public CostCodeReport()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// The origin of the "Budgeted", "Earned", and "Actual" hours is still unclear.  I thought I understood last time I talked to Grant, but the understanding
        /// seems to have slipped away again.  I'm just not clear on how the field is reporting the hours for each system/phase code without filling out a timecard.
        /// </remarks>
        public void GenerateCostCodeReport()
        {
            _costCodeDataService = new CostCodeDataService(_jobNumber);
            List<string> reportedPhaseCodes = new List<string>();
            List<PhaseCode> phaseCodeInstance = new List<PhaseCode>();

            //For each reported system, the estimated hours (for each reported phase code) for that system need to be read and assigned from the Estimate sheet.
            //GenerateReportedSystemEstimateData
            if (_reportModel != null)
            {
                foreach (SystemReport s in _reportModel.Systems)
                {
                    foreach (PhaseCode p in s.PhaseCodes)
                    {
                        //Assign the hours to the phase code's "EstimatedHours" property.
                        p.EstimatedHours = _costCodeDataService.GetEstimatedHoursByPhaseCode(p.FullPhaseCode);
                        
                        //Log the phase code to the list of all reported phase codes; this list is used below to obtain the budgeted/earned hour values.
                        if(!reportedPhaseCodes.Contains(p.FullPhaseCode))
                        {
                            reportedPhaseCodes.Add(p.FullPhaseCode);
                        }

                        //Finally, add the phase code instance to the list of phase code instances; this is used below to tally all the actual hours by phase code
                        //in order to compare to the "earned"hours amount stored in the CostCodeResult.  Rather than iterate through the SystemReport list again, 
                        //a LINQ query will be used on the PhaseCode instance list. 
                        phaseCodeInstance.Add(p);
                    }
                }

                //Use the list of reported PhaseCodes to read and assign the total budgeted hours for that phase code.  
                //This is the information that comes from SPECTRUM or Powertrack.
                if(reportedPhaseCodes != null && reportedPhaseCodes.Count != 0)
                {
                    CostCodeResults = new List<CostCodeResult>();
                    //Instantiate the CostCodeResult objects by reading the phase code name 
                    foreach(string p in reportedPhaseCodes)
                    {
                        CostCodeResults.Add(new CostCodeResult(p.ToString())
                        {
                            //Assign comparison hours to the "EarnedHours" property.
                            EarnedHours = _costCodeDataService.GetBudgetedHoursByPhaseCode(p.ToString())
                        });

                    }
                }
                else
                {
                    throw new Exception("The list of reported phase codes failed to populate.  Please reference CostCodeReport.GenerateCostCodeReport() for more information.");
                }


                //For each phase code, the hours from the reported systems list must be tallied before being assigned to 'CostCodeResult.ActualHours'
                if(phaseCodeInstance!=null && phaseCodeInstance.Count != 0)
                {
                    foreach(CostCodeResult c in CostCodeResults)
                    {
                        var actualHours = (from p in phaseCodeInstance
                                           where p.FullPhaseCode == c.PhaseCode
                                           select p.EstimatedHours).Sum();
                        c.ActualHours = (int)actualHours;
                    }
                }
                else
                {
                    throw new Exception("The list of phase code instances failed to populate.");
                }

                //The CostCodeResult objects now need to be post-processed to calculate the reporting parameters (percent complete, over/under, etc.)
                foreach (CostCodeResult c in CostCodeResults)
                {
                    if (c.EarnedHours != 0)
                    {
                        c.EarnedActualRatio =Math.Round(((Double)c.ActualHours / c.EarnedHours), 2);
                        c.ProjectionStatus = GetProjectionStatus(c.EarnedActualRatio);
                    }
                    else
                    {
                        throw new Exception($"Earned hours for {c.PhaseCode} failed to populate.  Please reference the CostCodeReport module to find out more.");
                    }
                }

            }

        }

        private CostCodeProjectionStatus GetProjectionStatus(double earnedActualRatio)
        {
          if(earnedActualRatio >= 0 && earnedActualRatio < 1)
            { 
                return CostCodeProjectionStatus.UnderProjection; 
            }
          else if(earnedActualRatio > 1)
            {
                return CostCodeProjectionStatus.OverProjection;
            }
          else 
            { 
                return CostCodeProjectionStatus.EqualToProjection; 
            }
        }
    }
}
