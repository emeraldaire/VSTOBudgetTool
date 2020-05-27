using Estimating.ProgressReporter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Estimating.VSTO.Reporting
{
    public class DataDisplayService
    {
        private ComparatorReport _comparatorReport { get; set; }

        public DataDisplayService()
        {

        }

        public static DataDisplayService LoadDataObject(ComparatorReport comparatorReport)
        {
            DataDisplayService dataDisplayService = new DataDisplayService();
            dataDisplayService._comparatorReport = comparatorReport;

            return dataDisplayService;
        }



    }
}
