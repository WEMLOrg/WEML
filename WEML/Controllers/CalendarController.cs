using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WEML.Models;
using WEML.Repos;

namespace WEML.Controllers
{
    public class DaySummaryViewModel
    {
        public DateTime Date { get; set; }
        public int SymptomsCount { get; set; }
        public int FeelingsCount { get; set; }
    }

    public class DayDetailsViewModel
    {
        public DateTime Date { get; set; }
        public IEnumerable<Symptom> Symptoms { get; set; }
        public IEnumerable<Feeling> Feelings { get; set; }
    }

    public class CalendarController : Controller
    {
        private readonly SymptomsRepo _symptomsRepo;
        private readonly FeelingsRepo _feelingsRepo;

        public CalendarController(SymptomsRepo symptomsRepo, FeelingsRepo feelingsRepo)
        {
            _symptomsRepo = symptomsRepo;
            _feelingsRepo = feelingsRepo;
        }

        public async Task<IActionResult> Index(int? year, int? month)
        {
            int currentYear = year ?? DateTime.Now.Year;
            int currentMonth = month ?? DateTime.Now.Month;

            var startDate = new DateTime(currentYear, currentMonth, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            var symptoms = await _symptomsRepo.GetSymptomsByDateRangeAsync(startDate, endDate);
            var feelings = await _feelingsRepo.GetFeelingsByDateRangeAsync(startDate, endDate);

            var symptomGroups = symptoms
                .GroupBy(s => s.DateTime.Date)
                .Select(group => new { Date = group.Key, Count = group.Count() });

            var feelingGroups = feelings
                .GroupBy(f => f.DateTime.Date)
                .Select(group => new { Date = group.Key, Count = group.Count() });

            var records = symptomGroups
                .Concat(feelingGroups)
                .GroupBy(g => g.Date)
                .Select(group => new DaySummaryViewModel
                {
                    Date = group.Key,
                    SymptomsCount = symptomGroups.FirstOrDefault(g => g.Date == group.Key)?.Count ?? 0,
                    FeelingsCount = feelingGroups.FirstOrDefault(g => g.Date == group.Key)?.Count ?? 0
                })
                .OrderBy(g => g.Date)
                .ToList();

            ViewData["Year"] = currentYear;
            ViewData["Month"] = currentMonth;

            return View(records);
        }

        public async Task<IActionResult> DetailsOfDate(DateTime date)
        {
            if (date == default)
            {
                return NotFound();
            }

            var symptoms = await _symptomsRepo.GetAllSymptomsByDate(date);
            var feelings = await _feelingsRepo.GetAllFeelingsByDate(date);

            var viewModel = new DayDetailsViewModel
            {
                Date = date,
                Symptoms = symptoms,
                Feelings = feelings
            };

            return View(viewModel);
        }
    }
}
