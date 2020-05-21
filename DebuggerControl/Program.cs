using Estimating.ProgressReporter.Interfaces.Model;
using Estimating.ProgressReporter.Model;
using Estimating.ProgressReporter.Services;
using Estimating.PseudoDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Estimating.CSVHandler;
using System.IO;
using CsvHelper;
using System.Globalization;

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
            ////Uncomment the two lines below to run the walkthrough example.
            WalkthroughExample walkthrough = new WalkthroughExample();
            walkthrough.PerformWalkthroughExample();


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
    }

}
