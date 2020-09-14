using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using PassportVerification;

namespace PassportVerificationTests
{
    class MrzLine2ModelTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [TestCase("")]
        [TestCase(null)]
        [Author("Stephen Moss")]
        [Description("MRZ Line 2 Model - Null Argument Exception")]
        public void NullArgumentExceptionRaised(string mrzLine2)
        {
            Assert.Throws<ArgumentNullException>(() => new MrzLine2Model(mrzLine2));
        }

        [Test]
        [TestCase("L898902C93UTO6908061F9406236ZE184226B<<<<<1")] // 1 character too short
        [TestCase("l898902C93UTO6908061F9406236ZE184226B<<<<<14")] // contains lowercase char
        [TestCase("L898902C93UTO6908061F9406236ZE184226B <<<<14")] // contains space char
        [Author("Stephen Moss")]
        [Description("MRZ Line 2 Model - Argument Exception - Failed Regex")]
        public void ArgumentExceptionRaised(string mrzLine2)
        {
            Assert.Throws<ArgumentException>(() => new MrzLine2Model(mrzLine2));
        }

        [Test]
        [TestCase("L898902C93UTO6908061F9406236ZE184226B<<<<<14", "L898902C9")]
        [TestCase("L<<<<<<<<3UTO6908061F9406236ZE184226B<<<<<14", "L")]
        [TestCase("1<<<<<<<<3UTO6908061F9406236ZE184226B<<<<<14", "1")]
        [TestCase("1234567893UTO6908061F9406236ZE184226B<<<<<14", "123456789")]
        [Author("Stephen Moss")]
        [Description("MRZ Line 2 Model - Passport Number Field")]
        public void PassportNumberField(string mrzLine2, string passportNumber)
        {
            var model = new MrzLine2Model(mrzLine2);
            Assert.AreEqual(passportNumber, model.PassportNumber);
        }

        [Test]
        [TestCase("L898902C<3UTO6908061F9406236ZE184226B<<<<<14", 3)]
        [TestCase("L898902C98UTO6908061F9406236ZE184226B<<<<<14", 8)]
        [TestCase("L<<<<<<<<7UTO6908061F9406236ZE184226B<<<<<14", 7)]
        [Author("Stephen Moss")]
        [Description("MRZ Line 2 Model - Passport Number Check Digit Field")]
        public void PassportNumberCheckDigitField(string mrzLine2, byte passportNumberCheckDigit)
        {
            var model = new MrzLine2Model(mrzLine2);
            Assert.AreEqual(passportNumberCheckDigit, model.PassportNumberCheckDigit);
        }

        [Test]
        [TestCase("L898902C<3UTO6908061F9406236ZE184226B<<<<<14", "UTO")]
        [Author("Stephen Moss")]
        [Description("MRZ Line 2 Model - Nationality Field")]
        public void NationalityField(string mrzLine2, string nationality)
        {
            var model = new MrzLine2Model(mrzLine2);
            Assert.AreEqual(nationality, model.Nationality);
        }

        [Test]
        [TestCase("L898902C<3UTO6909311F9406236ZE184226B<<<<<14")]
        [TestCase("L898902C<3UTO6908061F9406316ZE184226B<<<<<14")]
        [Author("Stephen Moss")]
        [Description("MRZ Line 2 Model - Date of Birth Field - Argument Exception")]
        public void DateFieldArgumentExceptionRaised(string mrzLine2)
        {
            Assert.Throws<ArgumentException>(() => new MrzLine2Model(mrzLine2));
        }
    }
}
