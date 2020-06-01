using Estimating.ProgressReporter.Interfaces.Model;
using Estimating.ProgressReporter.Model;
using Estimating.ProgressReporter.Services;
using Estimating.PseudoDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Estimating.CSVHandler;
using System.IO;
using CsvHelper;
using System.Globalization;
using System.Configuration;
using System.Collections.Specialized;

namespace DebuggerControl
{
    public class Foo
    {
        public string Subject { get; set; }
        public string Space { get; set; }
       
    }

    public class Program
    {
        static void Main(string[] args)
        {
            // *************************************************************
            // Run through Configuration Manager scenarios.
            //GetConfigurationValues();
            //GetConfigurationValuesBySection();
            //GetConnectionStrings();
            //Console.ReadLine();
            // *************************************************************
            string phaseCode = "0001-0901";
            string prefix = phaseCode.Substring(0, 4);
            string suffix = phaseCode.Substring(5, 4);

            Console.ReadLine();
            ////Uncomment the two lines below to run the walkthrough example.
            //WalkthroughExample walkthrough = new WalkthroughExample();
            //walkthrough.PerformWalkthroughExample();


            ////Initial implementation of CSV procedure for reading report file.
            ////REQUIRES: Project reference to 'CSVHelper' by Josh 
            //using (var reader = new StreamReader(@"C:\Users\noahb\Desktop\Budget VSTO\BluebeamFieldReport.csv"))
            //using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            //{

            //    var records = csv.GetRecords<Foo>();
            //    //cast the object as a list to get its metadata.
            //    //List<Foo> recordList = records.Cast<Foo>().ToList();
            //    //List<Foo> viaLINQList = records.ToList();
            //    List<Foo> singleLineList = (csv.GetRecords<Foo>()).ToList();

            //    Console.WriteLine($"There were {singleLineList.Count} records contained in the file");
            //    foreach (Foo r in singleLineList)
            //    {
            //        string id = r.Subject;
            //        string name = r.Space;
            //        Console.WriteLine($"PhaseCode: {id}, System: {name}");

            //    }

            //    //var idList = from s in singleLineList
            //    //             where s.Id > 3
            //    //             select new { record = s.Name };

            //    //idList.ToList();

            //    //var filteredRecords = singleLineList.Where(s => s.Id > 1).Select(s => s);
            //    //foreach (Foo r in filteredRecords)
            //    //{
            //    //    Console.WriteLine($"Filtered Record: {r.Id}, {r.Name}");
            //    //}

            //    //foreach (var i in idList)
            //    //{
            //    //    Console.WriteLine($"Member: {i.record}");
            //    //}

            //    Console.ReadLine();

           

        }

        public static void GetConfigurationValues()
        {
            var title = ConfigurationManager.AppSettings["Title"];
            var language = ConfigurationManager.AppSettings["Language"];

            Console.WriteLine($"{title} has been created using the {language} language.");
        }

        public static void GetConfigurationValuesBySection()
        {
            //To access our custom section settings, we first use the "GetSection" method of the ConfigurationManager class.
            //This will return a NameValueCollection (see class definition) containing all available keys.  We can specify 
            //the collection we want, then query the contents after the collection is successfully returned to us. 
            var applicationSettings = ConfigurationManager.GetSection("ApplicationSettings") as NameValueCollection;
            if (applicationSettings.Count == 0)
            {
                throw new Exception("The program was either unable to find the custom configuration section 'ApplicationSettings' or the section was empty when it was located");
            }
            else
            {
                Console.WriteLine("SecretKey is: " + applicationSettings["SecretKey"]);

                //foreach(var key in applicationSettings.AllKeys)
                //{
                //    Console.WriteLine(key + " = " + applicationSettings[key]);
                //}
            }



        }

        public static void GetConfigurationValuesGroupsInSections()
        {

        }

        public static void GetConnectionStrings()
        {
            var estimatingDatabaseString = ConfigurationManager.ConnectionStrings["Estimating"];
            var siloDatabaseString = ConfigurationManager.ConnectionStrings["Silo"];

            Console.WriteLine($"The connection string for the Estimating database is: {estimatingDatabaseString}");
            Console.WriteLine($"The connection string for the Silo database is: {siloDatabaseString}");
        }

    }

}
