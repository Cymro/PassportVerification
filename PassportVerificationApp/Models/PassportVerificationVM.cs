using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace PassportVerificationApp.Models
{
    [Serializable]
    public class PassportVerificationVM
    {
        const string MrzLine2Regex = "([a-zA-Z0-9<]{9})([0-9])([a-zA-Z]{3})([0-9]{6})([0-9])([mfMF<])([0-9]{6})([0-9])([a-zA-Z0-9<]{14})([0-9])([0-9])";

        public PassportVerificationVM()
        {
            DateOfBirth = DateTime.Now.AddYears(-30);
            ExpirationDate = DateTime.Now.AddYears(1); ;
        }

        [Required]
        [RegularExpression(MrzLine2Regex, ErrorMessage ="MRZ Line 2 is not in the correct format.")]
        [Display(Name="MRZ (Line2)")]
        public string MrzLine2 { get; set; }

        [Display(Name = "Passport No.")]
        public string PassportNumber { get; set; }

        public string Nationality { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime? DateOfBirth { get; set; }

        public Constants.Gender? Gender { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Expiration Date")]
        public DateTime? ExpirationDate { get; set; }

    }
}