using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PassportVerificationApp.Models
{
    public class PassportVerificationResultVM
    {
        #region Constructor
        internal PassportVerificationResultVM(bool passportNumberCheckDigitValid,
                                        bool dateOfBirthCheckDigitValid,
                                        bool passportExpirationDateCheckDigitValid,
                                        bool personalNumberCheckDigitValid,
                                        bool finalCheckDigitValid,
                                        bool genderCrossChecked,
                                        bool dateOfBirthCrossChecked,
                                        bool passportExpirtaionDateCrossChecked,
                                        bool nationalityCrossChecked,
                                        bool passportNumberCrossChecked)
        {
            const string pass = "Pass";
            const string fail = "Fail";

            PassportNumberCheckDigitValid = passportNumberCheckDigitValid ? pass : fail;
            DateOfBirthCheckDigitValid = dateOfBirthCheckDigitValid ? pass : fail;
            PassportExpirationDateCheckDigitValid = passportExpirationDateCheckDigitValid ? pass : fail;
            PersonalNumberCheckDigitValid = personalNumberCheckDigitValid ? pass : fail;
            FinalCheckDigitValid = finalCheckDigitValid ? pass : fail;
            GenderCrossChecked = genderCrossChecked ? pass : fail;
            DateOfBirthCrossChecked = dateOfBirthCrossChecked ? pass : fail;
            PassportExpirtaionDateCrossChecked = passportExpirtaionDateCrossChecked ? pass : fail;
            NationalityCrossChecked = nationalityCrossChecked ? pass : fail;
            PassportNumberCrossChecked = passportNumberCrossChecked ? pass : fail;
        }
        #endregion

        #region Properties
        [Display(Name= "Passport Number Check Digit")]
        public string PassportNumberCheckDigitValid  { get; }

        [Display(Name = "Date of Birth Check Digit")]
        public string DateOfBirthCheckDigitValid { get; }

        [Display(Name = "Passport Expiration Check Digit")]
        public string PassportExpirationDateCheckDigitValid { get; }

        [Display(Name = "Personal Number Check Digit")]
        public string PersonalNumberCheckDigitValid { get; }

        [Display(Name = "Final Check Digit")]
        public string FinalCheckDigitValid { get; }

        [Display(Name = "Gender Cross Check")]
        public string GenderCrossChecked { get; }

        [Display(Name = "Date of Birth Cross Check")]
        public string DateOfBirthCrossChecked { get; }

        [Display(Name = "Passport Expiration Cross Check")]
        public string PassportExpirtaionDateCrossChecked { get; }

        [Display(Name = "Nationality Cross Check")]
        public string NationalityCrossChecked { get; }

        [Display(Name = "Passport Number Cross Check")]
        public string PassportNumberCrossChecked { get; }

        #endregion
    }
}