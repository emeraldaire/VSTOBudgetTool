using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Estimating.SQLService
{
    public class SQLControl
    {
        private string connectionString {get; set;}

        public SQLControl()
        {

        }

        public void ExecuteQuery(string table, string query)
        {
            MessageBox.Show("Executing database query");
        }


    }
}
