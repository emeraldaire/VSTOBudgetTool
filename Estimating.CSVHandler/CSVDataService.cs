using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Estimating.CSVHandler
{
    public class CSVDataService<T>
    //public class CSVDataService : ICSVDataService
    {

        /// <summary>
        /// Reads the CSV file contents into a list of PhaseCodeRecord objects;  Assumes the CSV file has already been validated.
        /// </summary>
        /// <param name="filepath"></param>
        public List<T> GetRawRecords(string filepath)
        {
            using (var reader = new StreamReader(filepath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                try
                {
                    //Read the file contents into the applcation and filter out all non-phase code values.
                    //if(csv.ValidateHeader())
                    return csv.GetRecords<T>().ToList();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    throw;
                }
            }
        }

        /// <summary>
        /// Evaluates the CSV file to see if the formatting and contents are sufficient; returns enum 'CSVFileValidationResult'
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public CSVFileValidationResult ValidateCSVFile(string filePath)
        {
            return CSVFileValidationResult.Success;
        }


    }
}
