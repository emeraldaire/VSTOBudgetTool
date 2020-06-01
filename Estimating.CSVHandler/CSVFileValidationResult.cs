using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estimating.CSVHandler
{
    public enum CSVFileValidationResult
    {
        Success, 
        KeyColumnsMissing, 
        HeadersMissing
    }
}
