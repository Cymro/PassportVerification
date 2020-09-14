using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassportVerificationApp
{
    internal class ErrorLogService
    {
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        internal static void LogError(Exception exception)
        {
            _log.Error(exception.ToString());
        }
    }
}