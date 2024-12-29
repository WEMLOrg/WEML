using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WEML.Models;
using WEML.Repos;

namespace WEML.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly SymptomsRepo _symptomsRepo;
        private readonly FeelingsRepo _feelingsRepo;

        public StatisticsController(SymptomsRepo symptomsRepo, FeelingsRepo feelingsRepo)
        {
            _symptomsRepo = symptomsRepo;
            _feelingsRepo = feelingsRepo;
        }


        public async Task<IActionResult> Index()
        {
            var fontSize = Request.Cookies["FontSize"] ?? "30";
            ViewData["FontSize"] = fontSize;
            ClaimsPrincipal currentUser = User;
            var symptoms = await _symptomsRepo.GetAllSymptomsAsync(currentUser);
            var feelings = await _feelingsRepo.GetAllFeelingsAsync(currentUser);

            var symptomStatistics = symptoms
                .GroupBy(s => new { Year = s.DateTime.Year, Month = s.DateTime.Month })
                .Select(group => new MonthlyStatistics
                {
                    MonthYear = new DateTime(group.Key.Year, group.Key.Month, 1),
                    Entries = group
                        .GroupBy(s => s.SymptomName)
                        .Select(symptomGroup => new Statistics
                        {
                            Name = symptomGroup.Key,
                            Count = symptomGroup.Count(),
                            AverageSeverity = symptomGroup.Average(s => ParseSeverity(s.Severity))
                        }).ToList()
                })
                .ToList();

            var feelingStatistics = feelings
                .GroupBy(f => new { Year = f.DateTime.Year, Month = f.DateTime.Month })
                .Select(group => new MonthlyStatistics
                {
                    MonthYear = new DateTime(group.Key.Year, group.Key.Month, 1),
                    Entries = group
                        .GroupBy(f => f.FeelingName)
                        .Select(feelingGroup => new Statistics
                        {
                            Name = feelingGroup.Key,
                            Count = feelingGroup.Count(),
                            AverageSeverity = feelingGroup.Average(f => ParseSeverity(f.FeelingSeverity))
                        }).ToList()
                })
                .ToList();

            var mostFrequentSymptom = symptoms
                .GroupBy(s => s.SymptomName)
                .OrderByDescending(group => group.Count())
                .Select(group => new Statistics
                {
                    Name = group.Key,
                    Count = group.Count(),
                    AverageSeverity = group.Average(s => ParseSeverity(s.Severity))
                })
                .FirstOrDefault();

            var mostFrequentFeeling = feelings
                .GroupBy(f => f.FeelingName)
                .OrderByDescending(group => group.Count())
                .Select(group => new Statistics
                {
                    Name = group.Key,
                    Count = group.Count(),
                    AverageSeverity = group.Average(f => ParseSeverity(f.FeelingSeverity))
                })
                .FirstOrDefault();

            var viewModel = new ConsolidatedStatisticsViewModel
            {
                SymptomStatistics = symptomStatistics,
                FeelingStatistics = feelingStatistics,
                MostFrequentSymptom = mostFrequentSymptom,
                MostFrequentFeeling = mostFrequentFeeling
            };

            return View(viewModel);
        }

        private double ParseSeverity(string severity)
        {
            return severity switch
            {
                "Mild" => 1,
                "Moderate" => 2,
                "Severe" => 3,
                _ => 0
            };
        }
    }

    public class Statistics
    {
        public string Name { get; set; }
        public int Count { get; set; }
        public double AverageSeverity { get; set; }
    }

    public class MonthlyStatistics
    {
        public DateTime MonthYear { get; set; }
        public List<Statistics> Entries { get; set; }
    }

    public class ConsolidatedStatisticsViewModel
    {
        public List<MonthlyStatistics> SymptomStatistics { get; set; }
        public List<MonthlyStatistics> FeelingStatistics { get; set; }
        public Statistics MostFrequentSymptom { get; set; }
        public Statistics MostFrequentFeeling { get; set; }
    }
}
