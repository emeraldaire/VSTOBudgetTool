using Estimating.ProgressReporter.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Estimating.VSTO.Helpers
{
    public enum JobNumberValidationResult
    {
        Success,
        NullOrEmpty,
        NonNumeric,
        IncorrectLength,
        NoEstimate
    }

    public class JobNumberValidation
    {
        public JobNumberValidationResult ValidationResult { get; }
        public bool IsValidJobNumber { get; }
        private List<string> _activeEstimateJobNumbers { get; set; }
        private JobNumberRepository _jobNumberRepository { get; set; }

        public JobNumberValidation(string jobNumber)
        {
            //Get the list of job numbers associated with active estimates. 
            _jobNumberRepository = new JobNumberRepository();
            _activeEstimateJobNumbers = _jobNumberRepository.GetAll();

            //Obtain amd handle the validation result.  Consider encapsulating this class into a custom error.
            ValidationResult = ValidateJobNumberEntry(jobNumber);
            IsValidJobNumber = IsValid(ValidationResult);
        }

        private JobNumberValidationResult ValidateJobNumberEntry(string jobNumber)
        {
            jobNumber = jobNumber.Trim();

            //Job Number field is blank.
            if (String.IsNullOrEmpty(jobNumber))
            {
                return JobNumberValidationResult.NullOrEmpty;
            }
            else if (!int.TryParse(jobNumber, out _))
            {
                return JobNumberValidationResult.NonNumeric;
            }
            else if (jobNumber.Length != 7)
            {
                return JobNumberValidationResult.IncorrectLength;
            }
            else if(!_activeEstimateJobNumbers.Contains(jobNumber))
            {
                return JobNumberValidationResult.NoEstimate;
            };

            return JobNumberValidationResult.Success;
        }

        private bool IsValid(JobNumberValidationResult validationResult)
        {
            switch (validationResult)
            {
                case JobNumberValidationResult.NullOrEmpty:
                    MessageBox.Show("Please enter a job number.", "Missing Job Number",MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                case JobNumberValidationResult.NonNumeric:
                    MessageBox.Show("Job numbers must be 7 digits with no hyphens, dashes, or non-numeric characters.  (Example: 2170507)", "Format Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                case JobNumberValidationResult.IncorrectLength:
                    MessageBox.Show("Job numbers must be 7 digits with no hyphens, dashes, or non-numeric characters.  (Example: 2170507)", "Format Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                case JobNumberValidationResult.NoEstimate:
                    MessageBox.Show("No estimate was found for this job number.  If this is a valid job number, please contact the Estimating department and try again after they save a record of the estimate data.", "No Estimate Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                case JobNumberValidationResult.Success:
                    return true;
                default:
                    return false;
            }
        }



    }
}
