using NUnit.Framework;
using PassportVerification;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PassportVerificationTests
{
    class MrzLine2ModelVerifyTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [TestCase("L898902C<3UTO6908061F9406236ZE184226B<<<<<14")]
        [Author("Stephen Moss")]
        [Description("MRZ Line 2 Model Verify - All Check Digits Pass")]
        public void AllCheckDigitsVerifyPass(string mrzLine2)
        {
            var model = new MrzLine2Model(mrzLine2);
            var result = model.Verify();

            Assert.AreEqual(true, result.DateOfBirthCheckDigitValid);
            Assert.AreEqual(true, result.PassportExpirationDateCheckDigitValid);
            Assert.AreEqual(true, result.PassportNumberCheckDigitValid);
            Assert.AreEqual(true, result.PersonalNumberCheckDigitValid);
            Assert.AreEqual(true, result.FinalCheckDigitValid);
        }

        [Test]
        [TestCase("L898902C<4UTO6908062F9406235ZE184226B<<<<<22")]
        [Author("Stephen Moss")]
        [Description("MRZ Line 2 Model Verify - All Check Digits Fail")]
        public void AllCheckDigitVerifyFail(string mrzLine2)
        {
            var model = new MrzLine2Model(mrzLine2);
            var result = model.Verify();

            Assert.AreEqual(false, result.DateOfBirthCheckDigitValid);
            Assert.AreEqual(false, result.PassportExpirationDateCheckDigitValid);
            Assert.AreEqual(false, result.PassportNumberCheckDigitValid);
            Assert.AreEqual(false, result.PersonalNumberCheckDigitValid);
            Assert.AreEqual(false, result.FinalCheckDigitValid);
        }

        [Test]
        [TestCase("L898902C<4UTO6908062F9406235ZE184226B<<<<<22", "L898902C", true)]
        [TestCase("L898902C<4UTO6908062F9406235ZE184226B<<<<<22", "L898902", false)]
        [Author("Stephen Moss")]
        [Description("MRZ Line 2 Model Verify - Passport number cross check - pass / fail")]
        public void PassportNumberCrossCheck(string mrzLine2, string comparePassportNumber, bool expectedResult)
        {
            var model = new MrzLine2Model(mrzLine2);
            var result = model.Verify(comparePassportNumber);

            Assert.AreEqual(expectedResult, result.PassportNumberCrossChecked);
        }

        [Test]
        [TestCase("L898902C<4UTO6908062F9406235ZE184226B<<<<<22", "UTO", true)]
        [TestCase("L898902C<4UTO6908062F9406235ZE184226B<<<<<22", "USA", false)]
        [Author("Stephen Moss")]
        [Description("MRZ Line 2 Model Verify - Nationality cross check - pass / fail")]
        public void NationalityCrossCheck(string mrzLine2, string compareNationality, bool expectedResult)
        {
            var model = new MrzLine2Model(mrzLine2);
            var result = model.Verify(nationality: compareNationality);

            Assert.AreEqual(expectedResult, result.NationalityCrossChecked);
        }

        [Test]
        [TestCase("L898902C<4UTO6908062F9406235ZE184226B<<<<<22", Constants.Gender.Female, true)]
        [TestCase("L898902C<4UTO6908062F9406235ZE184226B<<<<<22", Constants.Gender.Male, false)]
        [TestCase("L898902C<4UTO6908062M9406235ZE184226B<<<<<22", Constants.Gender.Male, true)]
        [TestCase("L898902C<4UTO6908062M9406235ZE184226B<<<<<22", Constants.Gender.Female, false)]
        [TestCase("L898902C<4UTO6908062<9406235ZE184226B<<<<<22", Constants.Gender.NotSpecified, true)]
        [TestCase("L898902C<4UTO6908062<9406235ZE184226B<<<<<22", Constants.Gender.Male, false)]
        [TestCase("L898902C<4UTO6908062<9406235ZE184226B<<<<<22", null, false)]
        [Author("Stephen Moss")]
        [Description("MRZ Line 2 Model Verify - Gender cross check - pass / fail")]
        public void GenderCrossCheck(string mrzLine2, Constants.Gender? compareGender, bool expectedResult)
        {
            var model = new MrzLine2Model(mrzLine2);
            var result = model.Verify(gender: compareGender);

            Assert.AreEqual(expectedResult, result.GenderCrossChecked);
        }

        [Test]
        [TestCase("L898902C<4UTO6908062F9406235ZE184226B<<<<<22", "1969-08-06", true)]
        [TestCase("L898902C<4UTO6908062F9406235ZE184226B<<<<<22", "1969-08-05", false)]
        [Author("Stephen Moss")]
        [Description("MRZ Line 2 Model Verify - Date of Birth cross check - pass / fail")]
        public void DateOfBirthCrossCheck(string mrzLine2, DateTime compareDateOfBirth, bool expectedResult)
        {
            var model = new MrzLine2Model(mrzLine2);
            var result = model.Verify(dateOfBirth: compareDateOfBirth);

            Assert.AreEqual(expectedResult, result.DateOfBirthCrossChecked);
        }

        [Test]
        [TestCase("L898902C<4UTO6908062F9406235ZE184226B<<<<<22", "1994-06-23", true)]
        [TestCase("L898902C<4UTO6908062F9406235ZE184226B<<<<<22", "1994-06-24", false)]
        [Author("Stephen Moss")]
        [Description("MRZ Line 2 Model Verify - Expiration Date cross check - pass / fail")]
        public void ExpirationDateCrossCheck(string mrzLine2, DateTime compareExpirationDate, bool expectedResult)
        {
            var model = new MrzLine2Model(mrzLine2);
            var result = model.Verify(expirationDate: compareExpirationDate);

            Assert.AreEqual(expectedResult, result.PassportExpirtaionDateCrossChecked);
        }
    }
}
