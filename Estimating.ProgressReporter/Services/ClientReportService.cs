using Estimating.ProgressReporter.Interfaces.Model;
using Estimating.ProgressReporter.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estimating.ProgressReporter.Services
{
    /// <summary>
    /// Primary service class for client.  This class oversees creation of repositories and report processing; its primary purpose is to generate and return a 
    /// queryable report object to the caller. 
    /// </summary>
    /// <remarks>
    /// DATE: 5/21/20
    /// AUTHOR: Noah B
    /// ABSTRACT: The primary inputs for this class are the report file path (string) and the job number for the report (string).  Obtaining and validating 
    /// these pieces of information is the responsibility of the caller.  The primary deliverable of this class is a queryable 'ComparatorReport' object which 
    /// the caller can use to pipe and display the data. 
    /// </remarks>
    public class ClientReportService 
    {
        public ComparatorReport GetReportSummary(string jobNumber, string filePath)
        {
            //Assign
            return null;
        }
    }
}
