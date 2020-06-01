using Estimating.CSVHandler;
using Estimating.ProgressReporter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Estimating.VSTO.Helpers
{
    public class CSVHelper
    {

        public List<SystemReport> GetReportedSystemList(string filePath)
        {
            //Instantiate the CSVDataService for the desired object type.
            CSVDataService<PhaseCodeHelper> csvDataService = new CSVDataService<PhaseCodeHelper>();   
            List<PhaseCodeHelper> filteredPhaseCodeList = FilterPhaseCodeRecords(csvDataService.GetRawRecords(filePath));
             return GetSystemReports(filteredPhaseCodeList);
        }

        /// <summary>
        /// Filters the provided list to keep only entries conforming to the Phase Code format defined in the Application Documentation; Returns a list of PhaseCodeHelper objects.
        /// </summary>
        /// <remarks>
        /// THe phase code formatting is '0000-0000' currently.
        /// </remarks>
        /// <param name="unfilteredPhaseCodeRecords"></param>
        /// <returns></returns>
        private List<PhaseCodeHelper> FilterPhaseCodeRecords(List<PhaseCodeHelper> unfilteredPhaseCodeRecords)
        {
            List<PhaseCodeHelper> filteredPhaseCodeRecords = new List<PhaseCodeHelper>();

            foreach (PhaseCodeHelper pc in unfilteredPhaseCodeRecords)
            {
                if (Regex.IsMatch(pc.Subject, @"^\d{4}-\d{4}$"))
                {
                    filteredPhaseCodeRecords.Add(pc);
                }
                else
                {
                    //MessageBox.Show($"Dumping {pc.Subject}");
                }
            }

            //MessageBox.Show($"There were {filteredPhaseCodeRecords.Count} phase code records contained in the file");
            return filteredPhaseCodeRecords;
        }

       
        /// <summary>
        /// Handles the filtered PhaseCodeHelper list to return a populated list of SystemReport objects; this is deliverable to the Main caller.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="reportedPhaseCodes"></param>
        /// <returns></returns>
        private List<SystemReport> GetSystemReports(List<PhaseCodeHelper> reportedPhaseCodes)
        {
            //1. List every system that is represented 
            //2. Create an empty SystemReport object for each SystemName in the list.
            //3. Convert the PhaseCodeHelper objects into PhaseCode objects.
            //4. Use LINQ query to populate the PhaseCode list for the associated system.  
            
            List<string> reportedSystemNames = new List<string>();
            List<PhaseCode> phaseCodeList = new List<PhaseCode>();
            List<SystemReport> systemReportList = new List<SystemReport>();

            foreach (PhaseCodeHelper p in reportedPhaseCodes)
            {
                //'Subject' corresponds to the phase code value 
                //'Space' corresponds to the system name
                //This is made explicit below instead of using a one line assignment for the new PhaseCode object.
                string phaseCodeValue = p.Subject;
                string systemName = p.Space;
                phaseCodeList.Add(new PhaseCode(phaseCodeValue, systemName));

                //If the system name does not already exist in the system name list, add it.  After the iteration terminates, the system name list
                //will contain single, unique entry for every named system in the report.  An alternative approach would be to create a Dictionary for
                //the system names, then convert the Dictionary to a pair-value list later.  The end result is the same: a list of system names with no
                //duplicates.
                if(!reportedSystemNames.Contains(p.Space))
                {
                    reportedSystemNames.Add(p.Space);
                }

            }

            //Finally, create the list of SystemReport objects.  The list of system names is used as the iteration control. 
            foreach(string s in reportedSystemNames)
            {
                string system = s;
                var phaseCodeQuery = phaseCodeList.Where(pc => pc.AssociatedSystem == s).Select(pc => pc).ToList();
                List<PhaseCode> systemPhaseCodes = new List<PhaseCode>(phaseCodeQuery);


                systemReportList.Add(
                    new SystemReport()
                    {
                        Name = s,
                        //PhaseCodes = (List<PhaseCode>)phaseCodeList.Where(pc => pc.AssociatedSystem == s.ToString()).Select(pc => pc)
                        PhaseCodes = systemPhaseCodes 
                    }) ; 
            }


            return systemReportList;
        }


    }
}
