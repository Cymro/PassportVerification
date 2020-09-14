using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassportVerification
{
    public static class Constants
    {
        public const string MrzLine2Regex = "([A-Z0-9<]{9})([0-9])([A-Z]{3})([0-9]{6})([0-9])([MF<])([0-9]{6})([0-9])([A-Z0-9<]{14})([0-9])([0-9])";
        public enum Gender
        {
            NotSpecified,
            Male,
            Female
        }
    }
}
