using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace PassportVerification
{
    /// <summary>
    /// Class to represent the MRZ Line 2 and its constituent parts
    /// 
    /// The class has been designed as immutable which makes for a more robust
    /// implementation.  Immutable classes are less open to abuse, as their state
    /// cannot be altered.
    /// </summary>
    public class MrzLine2Model
    {
        private const string FillerChar = "<";

        #region Constructors

        public MrzLine2Model(string mrzLine2)
        {
            //validate input
            if (string.IsNullOrEmpty(mrzLine2))
            {
                throw new ArgumentNullException();
            }
            if (!Regex.IsMatch(mrzLine2, Constants.MrzLine2Regex))
            {
                throw new ArgumentException($"MRZ Line 2 must be in format \"{Constants.MrzLine2Regex}\"");
            }

            /// Split the complete MRZ Line 2 into its separate fields
            /// Most validation is handled by the Regex, with only DateTime fields 
            /// requiring additional date checks, which will be performed in GetconvertedDate
            MrzLine2 = mrzLine2;
           
            _passportNumberRaw = mrzLine2.Substring(0, 9);
            PassportNumber = _passportNumberRaw.Replace(FillerChar, " ").TrimEnd();
            PassportNumberCheckDigit = Byte.Parse(mrzLine2.Substring(9, 1));

            Nationality = mrzLine2.Substring(10, 3).Replace(FillerChar, " ").TrimEnd();

            _dateOfBirthRaw = mrzLine2.Substring(13, 6);
            DateOfBirth = GetConvertedDate(_dateOfBirthRaw);
            DateOfBirthCheckDigit = Byte.Parse(mrzLine2.Substring(19, 1));

            switch (mrzLine2.Substring(20, 1))
            {
                case "M":
                    Gender = Constants.Gender.Male;
                    break;
                case "F":
                    Gender = Constants.Gender.Female;
                    break;
                default:
                    Gender = Constants.Gender.NotSpecified;
                    break;
            }

            _expirationDateRaw = mrzLine2.Substring(21, 6);
            ExpirationDate = GetConvertedDate(_expirationDateRaw);
            ExpirationDateCheckDigit = Byte.Parse(mrzLine2.Substring(27, 1));

            _personalNumberRaw = mrzLine2.Substring(28, 14);
            PersonalNumber = _personalNumberRaw.Replace(FillerChar, " ").TrimEnd();
            if (!string.IsNullOrWhiteSpace(PersonalNumber))
            {
                PersonalNumberCheckDigit = Byte.Parse(mrzLine2.Substring(42, 1));
            }

            FinalCheckDigit = Byte.Parse(mrzLine2.Substring(43, 1));
        }

        //function to convert date fields from yyMMdd.  On failure an ArgumentException will be thrown.
        private DateTime GetConvertedDate(string dateField)
        {
            const string dateFormat = "yyMMdd";

            DateTime convertedDate;
            if (DateTime.TryParseExact(dateField,
                                dateFormat,
                                System.Globalization.CultureInfo.InvariantCulture,
                                System.Globalization.DateTimeStyles.None,
                                out convertedDate))
            {
                return convertedDate;
            }
            else
            {
                throw new ArgumentException("Date field is not a valid date format. Must be in format YYMMDD");
            }
        }

        #endregion

        #region Fields

        //these internal representations of the properties are required for the check digit verification
        private readonly string _passportNumberRaw;
        private readonly string _dateOfBirthRaw;
        private readonly string _expirationDateRaw;
        private readonly string _personalNumberRaw;
        #endregion


        #region Properties

        /// <summary>
        /// Complete MrzLine2 - (Chars 1-44)
        /// </summary>
        public string MrzLine2 { get; }

        /// <summary>
        /// Passport Number - (Extracted from chars 1-9)
        /// </summary>
        public string PassportNumber { get; }

        /// <summary>
        /// Passport Number Check Digit - (Extracted from char 10)
        /// </summary>
        public byte PassportNumberCheckDigit { get; }

        /// <summary>
        /// Nationality Code - ISO-3166-1 alpha-3 with modifications - (Extracted from chars 11-13)
        /// </summary>
        public string Nationality { get; }

        /// <summary>
        /// Date of Birth - (Extracted from chars 14-19)
        /// </summary>
        public DateTime DateOfBirth { get; }

        /// <summary>
        /// Date of Birth Check Digit - (Extracted from char 20)
        /// </summary>
        public byte DateOfBirthCheckDigit { get; }

        /// <summary>
        /// Gender (M /F / null)- (Extracted from char 21)
        /// </summary>
        public Constants.Gender Gender { get; }

        /// <summary>
        /// Expiration Date - (Extracted from chars 22-27)
        /// </summary>
        public DateTime ExpirationDate { get; }

        /// <summary>
        /// Expiration Date Check Digit - (Extracted from char 28)
        /// </summary>
        public byte ExpirationDateCheckDigit { get; }

        public string PersonalNumber { get; }

        public byte? PersonalNumberCheckDigit { get; }

        /// <summary>
        /// Fina Check Digit - (Extracted from char 44)
        /// </summary>
        public byte FinalCheckDigit { get; }

        #endregion

        #region Methods

        public MrzLine2VerificationResult Verify(string passportNumber=null, 
                                                    string nationality=null, 
                                                    DateTime? dateOfBirth=null,
                                                    Constants.Gender? gender=null,
                                                    DateTime? expirationDate=null)
        {

            //StringBuilder would be of no use here with a fixed number 
            var finalCheckDigitData = string.Concat(_passportNumberRaw,
                                                       PassportNumberCheckDigit,
                                                       _dateOfBirthRaw,
                                                       DateOfBirthCheckDigit,
                                                       _expirationDateRaw,
                                                       ExpirationDateCheckDigit,
                                                       _personalNumberRaw,
                                                       PersonalNumberCheckDigit.HasValue ? PersonalNumberCheckDigit.Value.ToString() : FillerChar
                                                   );

            return new MrzLine2VerificationResult(PassportNumberCheckDigit == PassportCheckDigit.GetCheckDigit(_passportNumberRaw),
                                                    DateOfBirthCheckDigit == PassportCheckDigit.GetCheckDigit(_dateOfBirthRaw),
                                                    ExpirationDateCheckDigit == PassportCheckDigit.GetCheckDigit(_expirationDateRaw),
                                                    PersonalNumberCheckDigit == PassportCheckDigit.GetCheckDigit(_personalNumberRaw),
                                                    FinalCheckDigit == PassportCheckDigit.GetCheckDigit(finalCheckDigitData),
                                                    Gender == gender, 
                                                    DateOfBirth == dateOfBirth,
                                                    ExpirationDate == expirationDate,
                                                    Nationality == nationality,
                                                    PassportNumber == passportNumber
                                                  );
        }

        #endregion
    }
}
