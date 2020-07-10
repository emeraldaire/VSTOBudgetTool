using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estimating.Cache.Interfaces
{
    public interface IHoursEntry
    {
        string JobNumber { get; set; }
        string PhaseCode { get; set; }
        double Hours { get; set; }

       
    }
}
