using System;
using System.Drawing;

namespace PassportVerificationApi
{
    internal class ErrorLogService
    {
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        internal static void LogError(Exception exception)
        {
            _log.Error(exception.ToString());
        }
        internal static void LogWarning(string warningMessage)
        {
            if (_log.IsWarnEnabled)
                _log.Warn(warningMessage);
        }
    }
}