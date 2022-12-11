using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RTools_NTS.Util;
using System.Diagnostics.Eventing.Reader;
using System.Net;
using System.Web.Http;

namespace JWTAPI.Controllers
{
    [Route("api/captcha")]
    [ApiController]
    public class Captcha : ControllerBase
    {


        [HttpPost]
        public async Task<ActionResult> VerifyRecaptchaAsync(string responseToken)
        {
            // Utwórz obiekt do wysyłania zapytania HTTP
            var client = new HttpClient();

            // Skonfiguruj zapytanie do serwera reCaptcha
            var request = new HttpRequestMessage(HttpMethod.Post, "https://www.google.com/recaptcha/api/siteverify");
            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "secret", "6LfWQ2wjAAAAAKDOgkgDm5v7RpDfBUO-iBD5mhMA" },
            { "response", responseToken },
        });


            // Wykonaj zapytanie do serwera reCaptcha
            var response = client.SendAsync(request);
            var responseString = response.Result.Content.ReadAsStringAsync();


            // Parsuj odpowiedź serwera jako obiekt JSON
            var json = await Task.Run(() => JsonConvert.DeserializeObject<dynamic>(responseString.Result));

            var success =(bool) json["success"];    
            // Sprawdź wynik weryfikacji
            if (success)
            {
                // ReCaptcha została pomyślnie przesłana
                return Ok();
            }
            else
            {
                return BadRequest(); 
                // ReCaptcha została odrzucona lub wystąpił błąd
             //   return BadRequest(json);
            }
        }
    }

}
