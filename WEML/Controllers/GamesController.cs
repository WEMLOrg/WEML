using WEML.Data;
using WEML.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using WEML.Areas.Identity.Data;

namespace WEML.Controllers
{
    public class GamesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public GamesController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> GetAllQuestionsForAChallange(int? id)
        {
            var fontSize = Request.Cookies["FontSize"] ?? "30"; 
            ViewData["FontSize"] = fontSize;
            if (id == null)
            {
                return NotFound();
            }

            var questionsIDs = await _context.ChallangeQuestions
                .Where(cq => cq.cId == id)
                .ToListAsync();

            if (!questionsIDs.Any())
            {
                return NotFound();
            }

            List<Question> allQuestions = new List<Question>();

            foreach (var questionID in questionsIDs)
            {
                var question = await _context.Questions
                    .FirstOrDefaultAsync(q => q.qId == questionID.questionId);
                if (question != null)
                {
                    allQuestions.Add(question);
                }
            }

            return View(allQuestions);
        }

        public async Task<IActionResult> GetAllChallanges()
        {
            var fontSize = Request.Cookies["FontSize"] ?? "30"; 
            ViewData["FontSize"] = fontSize;
            var challenges = await _context.Challanges.ToListAsync();
            return View(challenges);
        }

        public async Task<IActionResult> OpenChallange(int? id)
        {
            var fontSize = Request.Cookies["FontSize"] ?? "30"; 
            ViewData["FontSize"] = fontSize;
            if (id == null)
            {
                return NotFound();
            }
            var challange = await _context.Challanges.FirstOrDefaultAsync(m => m.cId == id);
            if (challange == null)
            {
                return NotFound();
            }
            var questionIDs = await _context.ChallangeQuestions
                .Where(cq => cq.cId == id)
                .Select(cq => cq.questionId)
                .ToListAsync();

            var questions = await _context.Questions
                .Where(q => questionIDs.Contains(q.qId))
                .ToListAsync();
            return View(questions); 
        }


        public async Task<IActionResult> GetQuestionById(int? id)
        {
            var fontSize = Request.Cookies["FontSize"] ?? "30"; 
            ViewData["FontSize"] = fontSize;
            if (id == null)
            {
                return NotFound();
            }

            var questions = await _context.Questions
                .FirstOrDefaultAsync(m => m.qId == id);
            if (questions == null)
            {
                return NotFound();
            }

            return View(questions);
        }
        private async Task<User?> GetUserAsync()
        {
            ClaimsPrincipal currentUser = User;
            return await _userManager.GetUserAsync(currentUser);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitAnswers(Dictionary<int, string> selectedAnswers)
        {
            var fontSize = Request.Cookies["FontSize"] ?? "30"; 
            ViewData["FontSize"] = fontSize;
            if (selectedAnswers == null || !selectedAnswers.Any())
            {
                ViewBag.ErrorMessage = "Please select an answer for each question.";
                return View("OpenChallange"); 

            }

            int totalPoints = 0;

            foreach (var questionId in selectedAnswers.Keys)
            {
                var question = await _context.Questions.FindAsync(questionId);
                if (question != null && question.correctAnswer == selectedAnswers[questionId])
                {
                    totalPoints += question.points;
                }
            }
            var user = await GetUserAsync();
            user.numberOfPoints += totalPoints;
            ViewBag.TotalPoints = totalPoints;
            
            return View("Results");
        }

    }
}
