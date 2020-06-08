using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estimating.ProgressReporter.Interfaces.Model
{
    //TODO: Complete the list of acceptable phase code types and categories.
    public enum PhaseCodeType
    {
        Demo,
        SafetyMatters,
        Contingency,
        ForemanTime,
        FieldLayout,
        MatHandlingShop,
        Fabrication,
        GRDInstall,
        EquipmentRental,
        JobTrailerRental,
        SpecialTeamsDetailing,
        SpecialTeamsRFRPipe,
        SpecialTeamsControls,
        CondensatePipe,
        StartTest,
        AirBalance,
        GasPipe,
        OwnerTraining,
        Commissioning,
        Insulation,
        Controls,
        Electrical,
        RoofGC,
        Piping,
        Crane,
        Engineering,
        SawCutterCoring,
        FireStopping,
        DuctInstallation,
        EquipmentInstall,
        PermitFees,
        BIM,
        WarrantyReserve,
        Indirects,
        TrustMoney,
        EquityMoney,
        Punchlist,
        SubsistenceTravel,
        Parking,
        Custom
    }

    public enum PhaseCodeCategory
    {
        //9900 
        ZBCode,
        //0004,
        Residential,
        //0003
        LightCommercial,
        //0001,
        BuildingTrades,
        //9000
        ChangeOrder,
        //Other
        Unclassified
    }

    /// <summary>
    /// Represents the information associated with a single phase code with respect to an equipment system.
    /// </summary>
    /// <remarks>
    /// While the Equipment System is broken into 'Estimate' and 'Report' classes, the IPhaseCode object is used by both ends of the process;  in other words, 
    /// when populating active application data, there is only one phasecode object available to the Estimate and Report processes.
    /// </remarks>
    public interface IPhaseCode
    {
        string AssociatedSystem { get; set; }
        string FullPhaseCode { get; }
        string PhaseCodeSuffix { get;}
        string PhaseCodePrefix { get; }
        PhaseCodeCategory PhaseCodeCategory { get;}
        PhaseCodeType PhaseCodeType { get; }
        double EarnedHours { get; set; }
        DateTime DateReported { get; }

    }
}
