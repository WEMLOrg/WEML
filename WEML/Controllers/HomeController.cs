using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WEML.Models;
using WEML.Repos;

namespace WEML.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private SymptomsRepo _symptomsRepo;

        public HomeController(ILogger<HomeController> logger, SymptomsRepo symptomsRepo)
        {
            _logger = logger;
            _symptomsRepo = symptomsRepo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
        public IActionResult SymptomsForm()
        {
            return View();
        }

        public IActionResult FeelingsForm()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> SubmitSymptom(string SymptomName, string SymptomDescription, string Severity)
        {
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
                SymptomId = new Guid()
            };
            await _symptomsRepo.AddSymptomAsync(newSymtom);
            TempData["Message"] = "Symptom submitted successfully!";
            return RedirectToAction("Index");
        }
        
        [HttpPost]
        public IActionResult SubmitFeeling(string FeelingName, string FeelingDescription, string FeelingSeverity)
        {
            TempData["Message"] = "Feeling submitted successfully!";
            return RedirectToAction("Index");
        }


    }
}
