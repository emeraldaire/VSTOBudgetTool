﻿using Estimating.ProgressReporter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estimating.ProgressReporter.Services
{
    public interface IEstimateDataService
    {
        List<SystemEstimate> GetSystemEstimatesByJobNumber(string jobNumber);
    }
}
