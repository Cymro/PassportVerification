using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassportVerificationApp.Models
{
    /// <summary>
    /// Passport Verification API Result Output- this serves as the output data contract with the Web API
    /// </summary>
    public class PassportVerificationApiResultDTO
    {
        #region Properties
        public bool PassportNumberCheckDigitValid { get; set; }

        public bool DateOfBirthCheckDigitValid { get; set; }

        public bool PassportExpirationDateCheckDigitValid { get; set; }

        public bool PersonalNumberCheckDigitValid { get; set; }

        public bool FinalCheckDigitValid { get; set; }

        public bool GenderCrossChecked { get; set; }

        public bool DateOfBirthCrossChecked { get; set; }

        public bool PassportExpirtaionDateCrossChecked { get; set; }

        public bool NationalityCrossChecked { get; set; }

        public bool PassportNumberCrossChecked { get; set; }

        #endregion
    }
}