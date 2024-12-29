using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WEML.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using WEML.Models;
using WEML.Repos;
using System.Security.Claims;

namespace WEML.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<HomeController> _logger;
        private SymptomsRepo _symptomsRepo;
        private FeelingsRepo _feelingsRepo;
        private readonly DiagnosisEngine _diagnosisEngine;

        public HomeController(UserManager<User> userManager,ILogger<HomeController> logger, SymptomsRepo symptomsRepo, FeelingsRepo feelingsRepo, DiagnosisEngine diagnosisEngine)
        {
            _logger = logger;
            this._userManager = userManager;
            _symptomsRepo = symptomsRepo;
            _feelingsRepo = feelingsRepo;
            _diagnosisEngine = diagnosisEngine;
        }

        // public async Task<IActionResult> Index()
        // {
        //     var recentSymptoms = await _symptomsRepo.GetMostRecentSymptomsAsync();
        //     string diagnosis = await _diagnosisEngine.GetDiagnosisAsync();
        //     ViewData["Diagnosis"] = diagnosis;
        //     return View();
        // }

        [HttpPost]
        public IActionResult SaveFontSize(int fontSize)
        {
            // Save the font size in a cookie
            Response.Cookies.Append("FontSize", fontSize.ToString(), new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddDays(30) 
            });

            return Ok();
        }


        public async Task<IActionResult> Index()
        {
            var fontSize = Request.Cookies["FontSize"] ?? "30"; 
            ViewData["FontSize"] = fontSize;
            try
            {
                Console.WriteLine("in home controller");
                ClaimsPrincipal currentUser = User;
                Console.WriteLine("Home controller user: " + currentUser.Identity.Name);
                var recentSymptoms = await _symptomsRepo.GetMostRecentSymptomsAsync(currentUser);
                Console.WriteLine("Home controller recent symptoms  : " + recentSymptoms.Count);
                if (recentSymptoms != null && recentSymptoms.Any())
                {
                    Console.WriteLine("incerc sa aflu diagnosis");
                    string diagnosis = await _diagnosisEngine.GetDiagnosisAsync(recentSymptoms);
                    Console.WriteLine("Home controller diagnosis " + diagnosis);
                    ViewData["Diagnosis"] = diagnosis;
                }
                else
                {
                    ViewData["Diagnosis"] = "No symptoms found to generate a diagnosis.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating diagnosis");
                ViewData["Diagnosis"] = "An error occurred while generating a diagnosis.";
            }
            ViewData["UserID"]=_userManager.GetUserId(this.User);
            return View();
        }

        public IActionResult Privacy()
        {
            var fontSize = Request.Cookies["FontSize"] ?? "30"; 
            ViewData["FontSize"] = fontSize;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var fontSize = Request.Cookies["FontSize"] ?? "30"; 
            ViewData["FontSize"] = fontSize;
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
        public IActionResult SymptomsForm()
        {
            var fontSize = Request.Cookies["FontSize"] ?? "30"; 
            ViewData["FontSize"] = fontSize;
            return View();
        }

        public IActionResult FeelingsForm()
        {
            var fontSize = Request.Cookies["FontSize"] ?? "30"; 
            ViewData["FontSize"] = fontSize;
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> SubmitSymptom(string SymptomName, string SymptomDescription, string Severity)
        {
            var fontSize = Request.Cookies["FontSize"] ?? "30"; 
            ViewData["FontSize"] = fontSize;
            if (string.IsNullOrEmpty(SymptomName) || string.IsNullOrEmpty(SymptomDescription) || string.IsNullOrEmpty(Severity))
            {
                TempData["Message"] = "All fields are required.";
                return RedirectToAction("Index");
            }

            Symptom newSymtom = new Symptom
            {
                SymptomName = SymptomName,
                Severity = Severity,
                SymptomDescription = SymptomDescription,
                SymptomId = new Guid(),
                DateTime = DateTime.Now
            };
            ClaimsPrincipal currentUser = User;
            await _symptomsRepo.AddSymptomAsync(newSymtom, currentUser);
            TempData["Message"] = "Symptom submitted successfully!";
            return RedirectToAction("Index");
        }
        
        [HttpPost]
        public async Task<IActionResult> SubmitFeeling(string FeelingName, string FeelingDescription, string FeelingSeverity)
        {
            var fontSize = Request.Cookies["FontSize"] ?? "30"; 
            ViewData["FontSize"] = fontSize;
            if (string.IsNullOrEmpty(FeelingName) || string.IsNullOrEmpty(FeelingDescription) || string.IsNullOrEmpty(FeelingSeverity))
            {
                TempData["Message"] = "All fields are required.";
                return RedirectToAction("Index");
            }

            Feeling newFeeling = new Feeling
            {
                FeelingName = FeelingName,
                FeelingDescription = FeelingDescription,
                FeelingId = new Guid(),
                FeelingSeverity = FeelingSeverity,
                DateTime = DateTime.Now
            };
            ClaimsPrincipal currentUser = User;
            await _feelingsRepo.AddFeelingAsync(newFeeling, currentUser);
            
            TempData["Message"] = "Feeling submitted successfully!";
            return RedirectToAction("Index");
        }
        
        [HttpGet]
        public async Task<IActionResult> SearchSymptoms(string term)
        {
            var fontSize = Request.Cookies["FontSize"] ?? "30"; 
            ViewData["FontSize"] = fontSize;
            if (string.IsNullOrWhiteSpace(term))
                return Json(new List<string>());
            ClaimsPrincipal currentUser = User;
            var symptoms = await _symptomsRepo.GetAllSymptomsAsync(currentUser);
            var matches = symptoms
                .Where(s => s.SymptomName.Contains(term, StringComparison.OrdinalIgnoreCase))
                .Select(s => s.SymptomName)
                .Distinct()
                .Take(10) 
                .ToList();

            return Json(matches);
        }
        
        


    }
}
