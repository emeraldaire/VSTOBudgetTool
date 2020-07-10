using Estimating.Cache.Interfaces;
using Jumble.ExternalCacheManager.Model;
using SQLManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estimating.Cache.Services
{
    public class DataInterpreter<T> where T : IHoursEntry 
    {
        public List<T> LoadCachedDataList(DataTable dataTable)
        {
            List<T> dataObjectList = new List<T>();

            try
            {
                for (int i = 0; i <= dataTable.Rows.Count - 1; i++)
                {
                    T recordEntry = (T)Activator.CreateInstance(typeof(T));

                    string preTreatedJobNumber = dataTable.Rows[i].ItemArray[0].ToString();
                    string preTreatedPhaseCode = dataTable.Rows[i].ItemArray[1].ToString();

                    recordEntry.Hours = Convert.ToDouble(dataTable.Rows[i].ItemArray[2]);
                    recordEntry.PhaseCode = preTreatedPhaseCode.Insert(4, "-");

                    dataObjectList.Add(recordEntry);
                }

                return dataObjectList;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
        }
    }
}
