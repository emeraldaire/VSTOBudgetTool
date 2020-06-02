using Estimating.ProgressReporter.Interfaces.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estimating.ProgressReporter.Model
{
    public class PhaseCode : IPhaseCode
    {
        public PhaseCode(string fullPhaseCode)
        {
            FullPhaseCode = fullPhaseCode;
        }

        public PhaseCode(string fullPhaseCode, string associatedSystem)
        {
            FullPhaseCode = fullPhaseCode;
            AssociatedSystem = associatedSystem;
        }
        public string AssociatedSystem { get; set; }
        private string _fullPhaseCode;
        public string FullPhaseCode
        {
            get { return _fullPhaseCode; }
            set 
            {
                _fullPhaseCode = value;
                if (!String.IsNullOrEmpty(_fullPhaseCode))
                {
                    //Parse the phase code and assign prefix/suffix values. 
                    PhaseCodePrefix = FullPhaseCode.Substring(0, 4);
                    PhaseCodeSuffix = FullPhaseCode.Substring(5, 4);

                    //Assign the phase code types/categories using the prefix and suffix obtained above.
                    PhaseCodeCategory = AssignPhaseCodeCategory(PhaseCodePrefix);
                    PhaseCodeType = AssignPhaseCodeType(PhaseCodeSuffix);

                    //Set the time that the phase code was processed by the application.  This is the time when the user runs the report, 
                    //not the time when the original CSV file was generated.
                    DateReported = DateTime.Now;
                }
            }
        }

        public string PhaseCodeSuffix { get; private set; }
        public string PhaseCodePrefix { get; private set; }
        public PhaseCodeCategory PhaseCodeCategory { get; private set; }
        public PhaseCodeType PhaseCodeType { get; private set; }
        public double EstimatedHours { get; set; }
        public DateTime DateReported { get; private set; }

        private PhaseCodeCategory AssignPhaseCodeCategory(string phaseCodePrefix)
        {
            switch (phaseCodePrefix)
            {
                case "0001":
                    return PhaseCodeCategory.BuildingTrades;
                case "9001":
                    return PhaseCodeCategory.ChangeOrder;
                default:
                    return PhaseCodeCategory.Unclassified;
            }
        }

        private PhaseCodeType AssignPhaseCodeType(string phaseCodeSuffix)
        {
            switch(phaseCodeSuffix)
            {
                case "0201":
                    return PhaseCodeType.FieldLayout;
                case "0301":
                    return PhaseCodeType.MatHandlingShop;
                case "0401":
                    return PhaseCodeType.GRDInstall;
                case "0501":
                    return PhaseCodeType.EquipmentRental;
                case "0601":
                    return PhaseCodeType.SpecialTeamsDetailing;
                case "0701":
                    return PhaseCodeType.AirBalance;
                case "0801":
                    return PhaseCodeType.DuctInstallation;
                case "0901":
                    return PhaseCodeType.EquipmentInstall;
                default:
                    return PhaseCodeType.Custom;
            }
        }


    }
}
