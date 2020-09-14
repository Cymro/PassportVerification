using PassportVerificationApi.Models;
using PassportVerification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;

namespace PassportVerificationApi.Controllers
{
    public class PassportVerificationController : ApiController
    {

        // GET: api/Passport/5
        public IHttpActionResult Get([FromUri]PassportVerificationInputDTO parameters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var model = new MrzLine2Model(parameters.MrzLine2);

            return Ok(model.Verify(parameters.PassportNumber,
                                    parameters.Nationality,
                                    parameters.DOB,
                                    parameters.Gender,
                                    parameters.ExpirationDate));
        }

        // POST: api/Passport
        public IHttpActionResult Post([FromBody]PassportVerificationInputDTO parameters)
        {
            if (!ModelState.IsValid)
            {
                var validationErrorReport = new System.Text.StringBuilder();
                validationErrorReport.AppendLine("Invalid Passport Verification Input");
                foreach (var inputParam in ModelState)
                {
                    validationErrorReport.AppendLine(inputParam.Key);
                    validationErrorReport.AppendLine($"Value: {inputParam.Value.Value}");
                    foreach(var modelError in inputParam.Value.Errors)
                    {
                        validationErrorReport.AppendLine($"Error: {modelError.ErrorMessage}");
                    }
                }
                ErrorLogService.LogWarning(validationErrorReport.ToString());
                return BadRequest(ModelState);
            }

            var model = new MrzLine2Model(parameters.MrzLine2);

            return Ok(model.Verify(parameters.PassportNumber,
                                    parameters.Nationality,
                                    parameters.DOB,
                                    parameters.Gender,
                                    parameters.ExpirationDate));
        }
    }
}
