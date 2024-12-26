using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WEML.Areas.Identity.Data;
using WEML.Data;
using WEML.Models;
using WEML.Service;

namespace WEML.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private HealthArticlesService _articlesService;
        private List<Article> articles;
        private readonly UserManager<User> _userManager;

        public ArticlesController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _articlesService = new HealthArticlesService();
            _context = context;
            articles = new List<Article>();
            _userManager = userManager;

        }

        private async Task<User?> GetUserAsync()
        {
            ClaimsPrincipal currentUser = User;
            return await _userManager.GetUserAsync(currentUser);
        }

        private IQueryable<Symptom> GetUserSymptomsQuery(Guid userId)
        {
            return from symptom in _context.Symptoms
                   join symptomUser in _context.Set<SymptomUser>() on symptom.SymptomId equals symptomUser.SymptomId
                   where symptomUser.UserId == userId.ToString()
                   select symptom;
        }

        public async Task<IActionResult> Index()
        {
            
            var user = await GetUserAsync();

            if (user == null)
                return NotFound();

            //var symptoms = await _context.Symptoms.ToListAsync();
            var symptoms = await GetUserSymptomsQuery(user.UserId).ToListAsync();

            return View(await _articlesService.GetArticlesAsync(symptoms));
        }

        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var symptoms = await _context.Symptoms.ToListAsync();
            var articles = await _articlesService.GetArticlesAsync(symptoms);
            var article = articles.FirstOrDefault(a => a.Uri == id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }


    }
}