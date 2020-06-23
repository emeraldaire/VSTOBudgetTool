
                //Garage system is complete; all phase codes in the estimate are represented. 
                SystemReport garagefans = new SystemReport("GF-1")
                {
                    PhaseCodes = new List<PhaseCode>()
                {
                    new PhaseCode("0001-0701"),
                    new PhaseCode("0001-0601"),
                    new PhaseCode("0001-0401")
                }
                };

                //Partially complete; there were four phase codes in the estimate, but only two of them are being reported.
                SystemReport ETrashExhaust = new SystemReport("T-E")
                {
                    PhaseCodes = new List<PhaseCode>()
                {
                    new PhaseCode("0001-0701"),
                    new PhaseCode("0001-0601"),
                    new PhaseCode("0001-0501")
                },

                };

                //Partially complete; there were two phase codes in the estimate, but only one of them is being reported.
                SystemReport ECorridorSupply = new SystemReport("RTU-1")
                {
                    PhaseCodes = new List<PhaseCode>()
                {
                    new PhaseCode("0001-0701"),
                },

                };