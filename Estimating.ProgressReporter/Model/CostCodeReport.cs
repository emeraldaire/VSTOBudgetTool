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
        /// Generates the primary CostCodeReport properties; after successfully running, the payload objects will be accessible to the caller.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public void GenerateCostCodeReport()
        {
            _costCodeDataService = new CostCodeDataService(_jobNumber);
            List<string> reportedPhaseCodes = new List<string>();
            List<PhaseCode> phaseCodeInstance = new List<PhaseCode>();

            //EARNED HOURS
            //For each reported system, read the "EarnedHours" amount for the system's phase codes from the BudgetVSTO database.
            if (_reportModel != null)
            {
                foreach (SystemReport s in _reportModel.Systems)
                {
                    foreach (PhaseCode p in s.PhaseCodes)
                    {
                        //Assign the hours to the phase code's "EstimatedHours" property.  Estimated hours are synonymous with 'Earned' hours and are 
                        //read from the BudgetVSTO database record for the system being reported.  The hours are sent to the list of phase code instances.
                        //They will be summed by phase code later; for now, just read the phase code hours for the single system.
                        p.EarnedHours = _costCodeDataService.GetEarnedHours(s.Name, p.FullPhaseCode);
                        
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

                //BUDGETED HOURS & ACTUAL HOURS
                //Use the list of reported PhaseCodes to read and assign the total budgeted hours and actual hours for that phase code from SPECTRUM.  
                if(reportedPhaseCodes != null && reportedPhaseCodes.Count != 0)
                {
                    CostCodeResults = new List<CostCodeResult>();
                    //Instantiate the CostCodeResult objects by reading the phase code name 
                    foreach(string p in reportedPhaseCodes)
                    {
                        CostCodeResults.Add(new CostCodeResult(p.ToString())
                        {
                            //Assign comparison hours to the "BudgetedHours" property.
                            BudgetedHours = _costCodeDataService.GetBudgetedHoursByPhaseCode(p.ToString()),
                            ActualHours = _costCodeDataService.GetActualHoursByPhaseCode(p.ToString())
                        });
                    }
                }
                else
                {
                    throw new Exception("The list of reported phase codes failed to populate.  Please reference CostCodeReport.GenerateCostCodeReport() for more information.");
                }

                //SUM THE EARNED HOURS 
                //For each phase code, the hours from the reported systems list must be tallied before being assigned to 'CostCodeResult.ActualHours'
                if(phaseCodeInstance!=null && phaseCodeInstance.Count != 0)
                {
                    foreach(CostCodeResult c in CostCodeResults)
                    {
                        var earnedHours = (from p in phaseCodeInstance
                                           where p.FullPhaseCode == c.PhaseCode
                                           select p.EarnedHours).Sum();
                        c.EarnedHours = (int)earnedHours;
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
