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
        private FeelingsRepo _feelingsRepo;
        private readonly DiagnosisEngine _diagnosisEngine;
        
        public HomeController(ILogger<HomeController> logger, SymptomsRepo symptomsRepo, FeelingsRepo feelingsRepo, DiagnosisEngine diagnosisEngine)
        {
            _logger = logger;
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
        
        public async Task<IActionResult> Index()
        {
            try
            {
                var recentSymptoms = await _symptomsRepo.GetMostRecentSymptomsAsync();

                if (recentSymptoms != null && recentSymptoms.Any())
                {
                    string diagnosis = await _diagnosisEngine.GetDiagnosisAsync(recentSymptoms);
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
                SymptomId = new Guid(),
                DateTime = DateTime.Now
            };
            await _symptomsRepo.AddSymptomAsync(newSymtom);
            TempData["Message"] = "Symptom submitted successfully!";
            return RedirectToAction("Index");
        }
        
        [HttpPost]
        public async Task<IActionResult> SubmitFeeling(string FeelingName, string FeelingDescription, string FeelingSeverity)
        {
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
            
            await _feelingsRepo.AddFeelingAsync(newFeeling);
            
            TempData["Message"] = "Feeling submitted successfully!";
            return RedirectToAction("Index");
        }
        
        [HttpGet]
        public async Task<IActionResult> SearchSymptoms(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
                return Json(new List<string>());

            var symptoms = await _symptomsRepo.GetAllSymptomsAsync();
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
