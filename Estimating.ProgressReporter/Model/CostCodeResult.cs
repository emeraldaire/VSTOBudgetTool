using Estimating.ProgressReporter.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estimating.ProgressReporter.Model
{
    public class CostCodeResult
    {
        public string PhaseCode { get; set; }

        //Budgeted Hours
        public int BudgetedHours { get; set; } = 0;
        //Earned hours are the budgeted hours from the Estimating sheet?
        public int EarnedHours { get; set; }
        //Actual hours are the billed hours read from the SPECTRUM (or Powertrack) table.
        private int _actualHours;

        public int ActualHours
        {
            get { return _actualHours; }
            set 
            { 
                _actualHours = value; 
            }
        }

        public CostCodeProjectionStatus ProjectionStatus { get; set; }

        public bool IsWorkCompleted { get; set; }
        //EarnedActualRatio represents the ratio of "Earned/Actual" hours, which should be distinguished from "Percent Complete"
        public double EarnedActualRatio { get; set; }
        public double PercentComplete { get; set; }

        //NOT CURRENTLY USED
        //The following two quantities are displayed on the Labor Reporting Chart but have not been integrated into the tool as of this date.  They are included 
        //here to assist future extensibility.
        public UnitOfMeasurement UnitOfMeasurement { get; set; }
        public int EstimatedUnitQuantity { get; set; }
        public int ActualUnitQuantity { get; set; }

        public CostCodeResult(string phaseCode)
        {
            PhaseCode = phaseCode;
        }
    }
}
