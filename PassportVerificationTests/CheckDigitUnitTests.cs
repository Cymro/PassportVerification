using NUnit.Framework;
using System;
using System.Diagnostics;
using PassportVerification;

namespace PassportVerificationTests
{
    public class CheckDigitUnitTests
    {
        [SetUp]
        public void Setup()
        {
            //warm the lib
            PassportVerification.PassportCheckDigit.GetCheckDigit("<0A");
            PassportVerification.PassportCheckDigit.GetCheckDigitAlternate("<0A");
        }

        [Test]
        [TestCase("AB2134<<<", 5)]
        [TestCase("L898902C<", 3)]
        [Author("Stephen Moss")]
        [Description("Passport Number Check Digit Checks")]
        public void PassportNumber(string passportNumber, int expectedCheckDigit)
        {
            Assert.AreEqual(expectedCheckDigit, PassportVerification.PassportCheckDigit.GetCheckDigit(passportNumber));
        }

        [Test]
        [TestCase("<0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ", 8)]
        [Author("Stephen Moss")]
        [Description("Check Digit Checks - All Valid Characters")]
        public void AllValidCharacter(string passportNumber, int expectedCheckDigit)
        {
            Assert.AreEqual(expectedCheckDigit, PassportVerification.PassportCheckDigit.GetCheckDigit(passportNumber));
        }


        [Test]
        [TestCase("690806", 1)]
        [TestCase("740411", 3)]
        [Author("Stephen Moss")]
        [Description("Check Digit Checks - Date fields (YYMMDD)")]
        public void DateStringYYMMDD(string dateString, int expectedCheckDigit)
        {
            Assert.AreEqual(expectedCheckDigit, PassportVerification.PassportCheckDigit.GetCheckDigit(dateString));
        }

        [Test]
        [TestCase("690806", 1)]
        [TestCase("<<<<<<<<<", 0)]
        [TestCase("1<", 7)]
        [Author("Stephen Moss")]
        [Description("Check Digit Checks - Miscellaneous")]
        public void MiscellaneousValues(string dateOfBirth, int expectedCheckDigit)
        {
            Assert.AreEqual(expectedCheckDigit, PassportVerification.PassportCheckDigit.GetCheckDigit(dateOfBirth));
        }

        [Test]
        [TestCase("", 0)]
        [TestCase(null, 0)]
        [Author("Stephen Moss")]
        [Description("Check Digit Checks - Null Argument Exception")]
        public void NullArgumentExceptionRaised(string emptyField, int expectedCheckDigit)
        {
            Assert.Throws<ArgumentNullException>(() => PassportCheckDigit.GetCheckDigit(emptyField));
        }

        [Test]
        [TestCase("a", 0)]
        [TestCase("ABC01^", 0)]
        [TestCase("<^", 0)]
        [Author("Stephen Moss")]
        [Description("Check Digit Checks - Argument Exception")]
        public void ArgumentExceptionRaised(string invalidField, int expectedCheckDigit)
        {
            Assert.Throws<ArgumentException>(() => PassportCheckDigit.GetCheckDigit(invalidField));
        }

        [Test]
        [TestCase("690806", 1)]
        [TestCase("<0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ", 8)]
        [TestCase("<<<<<<<<<", 0)]
        [TestCase("1<", 7)]
        [Author("Stephen Moss")]
        [Description("Check Digit Checks - Performance against alternate implementation")]
        public void PerformanceComparison(string misc, int expectedCheckDigit)
        {
            const int callCount = 10000;
            const int tickTolerancePerCall = 10;

            Stopwatch perfTimer = new Stopwatch();

            perfTimer.Start();
            for (int i = 0; i < callCount; i++)
            {   
                Assert.AreEqual(expectedCheckDigit, PassportVerification.PassportCheckDigit.GetCheckDigit(misc));
            }
            perfTimer.Stop();
            var currentImplementationElapsed = perfTimer.ElapsedTicks;

            perfTimer.Restart();
            for (int i = 0; i < callCount; i++)
            {
                Assert.AreEqual(expectedCheckDigit, PassportVerification.PassportCheckDigit.GetCheckDigitAlternate(misc));
            }
            perfTimer.Stop();
            var alternativeImplementationElapsed = perfTimer.ElapsedTicks;

            Warn.If(currentImplementationElapsed - (tickTolerancePerCall * callCount) > perfTimer.ElapsedTicks, ()=>$"Current:{currentImplementationElapsed}, Alternate:{alternativeImplementationElapsed}");
        }

    }
}