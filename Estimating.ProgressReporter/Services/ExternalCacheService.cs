using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SOM.BudgetVSTO.Enums;
using SOM.BudgetVSTO.Model;
using SOM.BudgetVSTO.Services;


namespace Estimating.ProgressReporter.Services
{
    public class ExternalCacheService : IExternalCacheService
    {
        private BudgetDataProvider _dataProvider { get; set;}
        private string _jobNumber { get; set; }

        private List<HoursEntry> actualHours { get; set; }
        private List<HoursEntry> projectedHours { get; set; }
        private List<HoursEntry> budgetedHours { get; set; }

        public ExternalCacheService(string jobNumber, BudgetDataProvider dataProvider )
        {
            _jobNumber = jobNumber;
            _dataProvider = dataProvider;
            FilterCache(jobNumber);
        }

        /// <summary>
        /// Attempt to filter the ECM files by jobnumber.  The files are asynchronously loaded when the program opens. 
        /// </summary>
        /// <param name="jobNumber"></param>
        /// <returns></returns>
        private void FilterCache(string jobNumber)
        {
            if(_dataProvider != null)
            {

                if (_dataProvider.CacheWasLoaded == true)
                {
                    try
                    {
                        projectedHours = (List<HoursEntry>)_dataProvider.projectedHours.Where(h => h.JobNumber == jobNumber).ToList();
                        actualHours = (List<HoursEntry>)_dataProvider.actualHours.Where(h => h.JobNumber == jobNumber).ToList();
                        budgetedHours = (List<HoursEntry>)_dataProvider.budgetedHours.Where(h => h.JobNumber == jobNumber).ToList();

                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message);
                    } 
                }
                else
                {
                    MessageBox.Show("The external data cache has not finished loading.  Please wait a minute and try again.");
                }

            }
            else
            {
                MessageBox.Show("There was a problem loading the external data cache.  Until the problem can be resolved, SPECTRUM will be the only " +
                    "available data source.  If SPECTRUM is currently locked by another user, you'll have to wait a few minutes and try to run the " +
                    "report again.");
            }

        }

        /// <summary>
        /// Return the summed actual hours from the phase code entry stored in the external data cache. 
        /// PhaseCode parameter must be formatted to be compatible with SPECTRUM.
        /// </summary>
        /// <param name="spectrumFormattedPhaseCode"></param>
        /// <returns></returns>
        public double GetActualHoursByPhaseCode(string spectrumFormattedPhaseCode)
        {
            try
            {
                HoursEntry entry = actualHours.Where(h => h.PhaseCode == spectrumFormattedPhaseCode).FirstOrDefault();
                if (entry != null)
                {
                    return entry.SummedHours;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return 0;
            }
        }

        /// <summary>
        /// Return the summed projected hours from the phase code entry stored in the external data cache. 
        /// PhaseCode parameter must be formatted to be compatible with SPECTRUM.
        /// </summary>
        /// <param name="spectrumFormattedPhaseCode"></param>
        /// <returns></returns>
        public double GetProjectedHoursByPhaseCode(string spectrumFormattedPhaseCode)
        {
            try
            {
                HoursEntry entry = projectedHours.Where(h => h.PhaseCode == spectrumFormattedPhaseCode).FirstOrDefault();
                if (entry != null)
                {
                    return entry.SummedHours;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return 0;
            }
        }
    }
}
