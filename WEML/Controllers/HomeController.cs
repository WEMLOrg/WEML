using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WEML.Diagnosis;
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

        public async Task<IActionResult> Index()
        {
            string diagnosis = await _diagnosisEngine.GetDiagnosisAsync();
            ViewData["Diagnosis"] = diagnosis;
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
                FeelingSeverity = FeelingSeverity
            };
            
            await _feelingsRepo.AddFeelingAsync(newFeeling);
            
            TempData["Message"] = "Feeling submitted successfully!";
            return RedirectToAction("Index");
        }


    }
}
