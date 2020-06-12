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
        private bool isSaveOperation { get; }
        public JobNumberValidation(string jobNumber, bool IsSaveOperation)
        {
            //Get the list of job numbers associated with active estimates. 
            _jobNumberRepository = new JobNumberRepository();
            isSaveOperation = IsSaveOperation; 

            //Ignore the active job number list if the operation is a save operation.
            if (isSaveOperation == false)
            {
                _activeEstimateJobNumbers = new List<string>();
                _activeEstimateJobNumbers = _jobNumberRepository.GetAll();
            }

            //Obtain amd handle the validation result.  Consider encapsulating this class into a custom error.
            ValidationResult = ValidateJobNumberEntry(jobNumber);
            IsValidJobNumber = IsValid(ValidationResult);
            //HasEstimateData = IsActiveJobNumber(ValidationResult);
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
            else if (isSaveOperation == false)
            {
                if ( _activeEstimateJobNumbers == null || !_activeEstimateJobNumbers.Contains(jobNumber))
                {
                    return JobNumberValidationResult.NoEstimate;
                }
            };

            return JobNumberValidationResult.Success;
        }

        /// <summary>
        /// Returns true if the job number meets all formatting requirements and false otherwise.  Also returns true for job numbers meeting formats, but not present
        /// in the Estimating database.  To test if a job number is represented in the Estimating database, use the 'IsActiveJobNumber' method in this class.
        /// </summary>
        /// <param name="validationResult"></param>
        /// <returns></returns>
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
                    return true;
                case JobNumberValidationResult.Success:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Returns false if the job number cannot be found in the Estimating database, which means the estimate data was never successfully
        /// committed to the record. NOTE: Returns true for every other case. Use 'IsValid' method of this class to trap other result status.
        /// This function is used when importing field reports.
        /// </summary>
        /// <param name="validationResult"></param>
        /// <returns></returns>
        public bool HasEstimateData(JobNumberValidationResult validationResult)
        {
            switch (validationResult)
            {
                case JobNumberValidationResult.NoEstimate:
                    MessageBox.Show("No estimate was found for this job number.  If this is a valid job number, please contact the Estimating department and try again after they save a record of the estimate data.", "No Comparison Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                case JobNumberValidationResult.Success:
                    return true;
                case JobNumberValidationResult.NullOrEmpty:
                    return true;
                case JobNumberValidationResult.NonNumeric:
                    return true;
                case JobNumberValidationResult.IncorrectLength:
                    return true;
                default:
                    return false;
            }
        }


    }
}
