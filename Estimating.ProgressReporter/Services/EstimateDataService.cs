using Estimating.ProgressReporter.Model;
using Estimating.SQLService;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estimating.ProgressReporter.Services
{
    public class EstimateDataService : IEstimateDataService
    {
        private List<SystemEstimate> _systemEstimateList;
        private SQLControl _sqlControl;

        public List<SystemEstimate> GetSystemEstimatesByJobNumber(string jobNumber)
        {
            //Get the query results from the SQLControl
            //_sqlControl.ExecuteQuery("EstimateMain", "SELECT....");
            //Iterate through the data results and populate the "List<SystemEstimate>" object that will be returned to the caller. 

            return GenerateFakeEstimateData();

        }

        private List<SystemEstimate> GenerateFakeEstimateData()
        {
            List<SystemEstimate> estimatedSystems = new List<SystemEstimate>();


            //GARAGE EXHAUST FANS
            SystemEstimate GarageFans = new SystemEstimate("GF-1")
            {
                PhaseCodes = new List<PhaseCode>()
                {
                    new PhaseCode("0001-0701"),
                    new PhaseCode("0001-0601"),
                    new PhaseCode("0001-0401")
                },

                Type = EquipmentSystemType.GarageExhaust
            };

            //EAST BUILDING TRASH EXHAUST
            SystemEstimate EASTTrashExhaust = new SystemEstimate("T-E")
            {
                PhaseCodes = new List<PhaseCode>()
                {
                    new PhaseCode("0001-0701"),
                    new PhaseCode("0001-0601"),
                    new PhaseCode("0001-0501"),
                    new PhaseCode("0001-0401")
                },

                Type = EquipmentSystemType.TrashExhaust
            };

            //WEST BUILDING TRASH EXHAUST
            SystemEstimate WESTTrashExhaust = new SystemEstimate("T-W")
            {
                PhaseCodes = new List<PhaseCode>()
                {
                    new PhaseCode("0001-0701"),
                    new PhaseCode("0001-0401")
                },

                Type = EquipmentSystemType.TrashExhaust
            };

            //EAST CORRIDOR SUPPLY
            SystemEstimate EASTCorridorSupply = new SystemEstimate("RTU-1")
            {
                PhaseCodes = new List<PhaseCode>()
                {
                    new PhaseCode("0001-0701"),
                    new PhaseCode("0001-0401")
                },

                Type = EquipmentSystemType.CorridorSystem
            };

            //WEST CORRIDOR SUPPLY
            SystemEstimate WESTCorridorSupply = new SystemEstimate("rtu-2")
            {
                PhaseCodes = new List<PhaseCode>()
                {
                    new PhaseCode("0001-0701"),
                    new PhaseCode("0001-0501"),
                    new PhaseCode("0001-0401")
                },

                Type = EquipmentSystemType.CorridorSystem
            };

            estimatedSystems.Add(GarageFans);
            estimatedSystems.Add(EASTCorridorSupply);
            estimatedSystems.Add(EASTTrashExhaust);
            estimatedSystems.Add(WESTCorridorSupply);
            estimatedSystems.Add(WESTTrashExhaust);

            return estimatedSystems;

        }
    }
}
