using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassportVerification
{
    /// <summary>
    /// Read Only MRZ Line 2 Verification Results.
    /// Class is immutable so that results 
    /// </summary>
    public class MrzLine2VerificationResult
    {
        #region Constructor

        //internal constructor to prevent creation outside of lib 
        internal MrzLine2VerificationResult(bool passportNumberCheckDigitValid,
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
            PassportNumberCheckDigitValid = passportNumberCheckDigitValid;
            DateOfBirthCheckDigitValid = dateOfBirthCheckDigitValid;
            PassportExpirationDateCheckDigitValid = passportExpirationDateCheckDigitValid;
            PersonalNumberCheckDigitValid = personalNumberCheckDigitValid;
            FinalCheckDigitValid = finalCheckDigitValid;
            GenderCrossChecked = genderCrossChecked;
            DateOfBirthCrossChecked = dateOfBirthCrossChecked;
            PassportExpirtaionDateCrossChecked = passportExpirtaionDateCrossChecked;
            NationalityCrossChecked = nationalityCrossChecked;
            PassportNumberCrossChecked = passportNumberCrossChecked;
        }

        #endregion

        #region Properties
        public bool PassportNumberCheckDigitValid { get; }

        public bool DateOfBirthCheckDigitValid { get; }

        public bool PassportExpirationDateCheckDigitValid { get; }

        public bool PersonalNumberCheckDigitValid { get;  }

        public bool FinalCheckDigitValid { get;  }

        public bool GenderCrossChecked { get;  }

        public bool DateOfBirthCrossChecked { get;  }

        public bool PassportExpirtaionDateCrossChecked { get;  }

        public bool NationalityCrossChecked { get;  }

        public bool PassportNumberCrossChecked { get;  }

        #endregion
    }
}
