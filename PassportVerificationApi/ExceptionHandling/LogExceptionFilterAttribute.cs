using System.Web.Http.Filters;

namespace PassportVerificationApi
{
    public class LogExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            ErrorLogService.LogError(context.Exception);
        }
    }
}