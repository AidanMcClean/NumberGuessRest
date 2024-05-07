using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Xml.Linq;

namespace NumberGuessAppNET.Controllers
{
    public class HomeController : Controller
    {
        private static readonly HttpClient _client = new HttpClient();

        public ActionResult Index()
        {
            ViewBag.AttemptCount = Session["AttemptCount"] ?? 0;
            ViewBag.Result = Session["Result"] ?? string.Empty;
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> SecretNumber(int lowerLimit, int upperLimit)
        {
            //response to service
            var response = await _client.GetAsync($"http://localhost:50483/Service1.svc/SecretNumber/{lowerLimit}/{upperLimit}");
            if (response.IsSuccessStatusCode)
            {
                var secretNumberXml = await response.Content.ReadAsStringAsync();
                var secretNumber = XElement.Parse(secretNumberXml).Value;
                //stores secret number in session and intializes attempts to 0
                Session["SecretNumber"] = secretNumber;
                Session["AttemptCount"] = 0;
                return Json(new { secretNumber });
            }
            return Json(new { error = "secret number not generated" });
        }

        [HttpPost]
        public async Task<JsonResult> CheckNumber(int userGuess)
        {
            //fetch secretnumber from the session
            var secretNumber = Session["SecretNumber"]?.ToString();
            //response to service
            var response = await _client.GetAsync($"http://localhost:50483/Service1.svc/CheckNumber/{userGuess}/{secretNumber}");
            if (response.IsSuccessStatusCode)
            {
                var resultXml = await response.Content.ReadAsStringAsync();
                var result = XElement.Parse(resultXml).Value;
                //increments attempt count in session
                Session["AttemptCount"] = (int)(Session["AttemptCount"] ?? 0) + 1;
                return Json(new { result = result, attemptCount = Session["AttemptCount"] });
            }
            return Json(new { result = "couldnt check guess", attemptCount = Session["AttemptCount"] });
        }
    }
}
