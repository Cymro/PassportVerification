using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PassportVerificationApp
{
    public static class Constants
    {
        public enum Gender
        {
            [Display(Name= "Not Specified")]
            NotSpecified,
            Male,
            Female
        }
    }
}