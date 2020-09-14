using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PassportVerification;

namespace PassportVerificationApi.Models
{
    /// <summary>
    /// Passport Verification Input paramaters
    /// Other than the MRZ Line 2 all other parameters are optional to allow partial verification.
    /// </summary>
    public class PassportVerificationInputDTO
    {
        //Formats are extended to allow lowercase a-z, which will be converted to uppercase here, in the model.
        const string MrzLine2Regex = "([a-zA-Z0-9<]{9})([0-9])([a-zA-Z]{3})([0-9]{6})([0-9])([mfMF<])([0-9]{6})([0-9])([a-zA-Z0-9<]{14})([0-9])([0-9])";
        const string GenderRegex = "([mfMF])";
        const string NationalityRegex = "([a-zA-Z<]+)";
        const string PassportNumberRegex = "([a-zA-Z0-9<]+)";

        private string _mrzLine2;
        [Required]
        [RegularExpression(MrzLine2Regex)]
        public string MrzLine2
        {
            get
            {
                return _mrzLine2;
            }
            set
            {
                _mrzLine2 = value?.ToUpper();
            }
        }

        public Constants.Gender? Gender { get; set; }

        public DateTime? DOB { get; set; }

        public DateTime? ExpirationDate { get; set; }

        private string _nationality;
        [StringLength(3, MinimumLength = 3)]
        [RegularExpression(NationalityRegex)]
        public string Nationality
        {
            get
            {
                return _nationality;
            }
            set
            {
                _nationality = value?.ToUpper();
            }
        }

        private string _passportNumber;
        [MaxLength(9)]
        [RegularExpression(PassportNumberRegex)]
        public string PassportNumber
        {
            get
            {
                return _passportNumber;
            }
            set
            {
                _passportNumber = value?.ToUpper();
            }
        }
    }
}