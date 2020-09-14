using System;
using System.Collections.Generic;
using System.Web.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Net.Http.Formatting;
using Newtonsoft.Json;
using System.Text;

namespace PassportVerificationApp.Controllers
{
    public class PassportVerificationController : Controller
    {

        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //more performant to have a static httpClient and call methods Async from this
        private static HttpClient _apiClient;
        static PassportVerificationController()
        {
            var baseUri = WebConfigurationManager.AppSettings["PassportVerificationApiUri"];

            _apiClient = new HttpClient();
            _apiClient.BaseAddress = new Uri(baseUri);
            _apiClient.DefaultRequestHeaders.Accept.Clear();
            _apiClient.DefaultRequestHeaders.Accept.Add(
                        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        // GET: SumissionForm
        public ActionResult Form()
        {
            try
            {
                ViewBag.Message = "Passport Details";
                return View();
            }
            catch (Exception ex)
            {
                ErrorLogService.LogError(ex);
                throw;
            }
        }

        // POST: SumissionForm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Validate(Models.PassportVerificationVM passportData)
        {
            Models.PassportVerificationResultVM result = null;

            //map to a DTO to maintain a decoupling of the Web API to our ViewModel.
            var apiInput = new Models.PassportVerificationInputDTO(passportData.MrzLine2,
                                                                     passportData.Gender,
                                                                     passportData.DateOfBirth,
                                                                     passportData.ExpirationDate,
                                                                     passportData.Nationality,
                                                                     passportData.PassportNumber);

            try
            {
                HttpResponseMessage response;
                using (var request = new HttpRequestMessage(HttpMethod.Post, ""))
                {
                    var requestContentBody = JsonConvert.SerializeObject(apiInput);
                    using (var content = new StringContent(requestContentBody,
                                                                Encoding.UTF8,
                                                                "application/json"))
                    {
                        request.Content = content;
                        response = await _apiClient.SendAsync(request);
                    }
                }

                if (response.IsSuccessStatusCode)
                {
                    var apiResult = await response.Content.ReadAsAsync<Models.PassportVerificationApiResultDTO>(new List<MediaTypeFormatter>() { new JsonMediaTypeFormatter() });
                    result = new Models.PassportVerificationResultVM(apiResult.PassportNumberCheckDigitValid,
                                                                        apiResult.DateOfBirthCheckDigitValid,
                                                                        apiResult.PassportExpirationDateCheckDigitValid,
                                                                        apiResult.PersonalNumberCheckDigitValid,
                                                                        apiResult.FinalCheckDigitValid,
                                                                        apiResult.GenderCrossChecked,
                                                                        apiResult.DateOfBirthCrossChecked,
                                                                        apiResult.PassportExpirtaionDateCrossChecked,
                                                                        apiResult.NationalityCrossChecked,
                                                                        apiResult.PassportNumberCrossChecked);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Unable to retrieve results from Passport Verification Service.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Unable to retrieve results from Passport Verification Service.");
                ErrorLogService.LogError(ex);
            }
            return View("Result", result);
        }

    }
}