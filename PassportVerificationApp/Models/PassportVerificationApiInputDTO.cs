using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassportVerificationApp.Models
{
    /// <summary>
    /// Passport Verification API Input paramaters - this serves as the input data contract with the Web API
    /// Other than the MRZ Line 2 all other parameters are optional to allow partial verification.
    /// </summary>
    public class PassportVerificationInputDTO
    {

        public PassportVerificationInputDTO(string mrzLine2, 
                                              Constants.Gender? gender,
                                              DateTime? dob,
                                              DateTime? expirationDate,
                                              string nationality,
                                              string passportNumber)
        {
            MrzLine2 = mrzLine2;
            Gender= gender;
            DOB = dob;
            ExpirationDate = expirationDate;
            Nationality = nationality;
            PassportNumber = passportNumber;
        }

        public string MrzLine2 { get; set; }

        public Constants.Gender? Gender { get; set; }

        public DateTime? DOB { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public string Nationality { get; set; }

        public string PassportNumber { get; set; }
    }
}