using Estimating.Cache.Interfaces;
using Jumble.ExternalCacheManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estimating.Cache.Model
{
    public class ProjectedHoursEntry : CacheObject, IHoursEntry
    {

        public string JobNumber { get; set; }
        public string PhaseCode { get; set; }
        public double Hours { get; set; }

        //PARAMETERLESS CONSTRUCTOR
        public ProjectedHoursEntry()
        {

        }

    }
}
