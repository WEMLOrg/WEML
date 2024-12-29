
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WEML.Areas.Identity.Data;
using WEML.Data;
using WEML.Models;
using WEML.Service;

namespace WEML.Repos
{
    public class SymptomsRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly DiagnosisEngine _diagnosisEngine;

        public SymptomsRepo(ApplicationDbContext context, UserManager<User> userManager, DiagnosisEngine diagnosisEngine)
        {
            _context = context;
            _userManager = userManager;
            _diagnosisEngine = diagnosisEngine;
        }

        private async Task<User?> GetUserAsync(ClaimsPrincipal currentUser)
        {
            return await _userManager.GetUserAsync(currentUser);
        }

        private IQueryable<Symptom> GetUserSymptomsQuery(string userId)
        {
            Console.WriteLine("USER ID normal: " + userId);
            Console.WriteLine("USER ID to string: " + userId.ToString());
            return from symptom in _context.Symptoms
                   join symptomUser in _context.SymptomUsers on symptom.SymptomId equals symptomUser.SymptomId
                   where symptomUser.UserId == userId
                   select symptom;
        }

        public async Task AddSymptomAsync(Symptom symptom, ClaimsPrincipal currentUser)
        {
            Console.WriteLine("IN ADD SYMPTOM");
            var user = await GetUserAsync(currentUser);
            Console.WriteLine("user u ii: " + user.FirstName);
            if (user == null)
                throw new InvalidOperationException("No logged-in user found.");

            if (symptom == null)
                throw new ArgumentNullException(nameof(symptom));

            _context.Symptoms.Add(symptom);

            if (user.SymptomUsers == null)
                user.SymptomUsers = new List<SymptomUser>();

            user.SymptomUsers.Add(new SymptomUser
            {
                SymptomId = symptom.SymptomId,
                UserId = user.UserId.ToString(),
            });

            await _context.SaveChangesAsync();

            var userSymptomCount = await GetUserSymptomsQuery(user.Id).CountAsync();
            if (userSymptomCount % 10 == 0)
            {
                var userSymptoms = await GetUserSymptomsQuery(user.Id).ToListAsync();
                SendStatusService sendStatus = new SendStatusService(user, userSymptoms);
                
            }
        }

        public async Task<List<Symptom>> GetAllSymptomsAsync(ClaimsPrincipal currentUser)
        {
            var user = await GetUserAsync(currentUser);

            if (user == null)
                throw new InvalidOperationException("No logged-in user found.");

            return await GetUserSymptomsQuery(user.Id).ToListAsync();
        }

        public async Task<IEnumerable<Symptom>> GetSymptomsByDateRangeAsync(DateTime startDate, DateTime endDate, ClaimsPrincipal currentUser)
        {
            var user = await GetUserAsync(currentUser);

            if (user == null)
                throw new InvalidOperationException("No logged-in user found.");

            return await GetUserSymptomsQuery(user.Id)
                .Where(s => s.DateTime >= startDate && s.DateTime <= endDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Symptom>> GetAllSymptomsByDate(DateTime date, ClaimsPrincipal currentUser)
        {
            var user = await GetUserAsync(currentUser);

            if (user == null)
                throw new InvalidOperationException("No logged-in user found.");

            return await GetUserSymptomsQuery(user.Id)
                .Where(s => s.DateTime.Date == date.Date)
                .ToListAsync();
        }

        private async Task<List<Symptom>> GetRecentSymptomsAsync(int count, string userId)
        {
            var query = await GetUserSymptomsQuery(userId)
                .OrderByDescending(s => s.DateTime)
                .Take(count)
                .ToListAsync();
            Console.WriteLine(query.ToList().Count + " recent sympoms");
            return query;
        }

        public async Task<List<string>> GetMostRecentSymptomsAsync(ClaimsPrincipal currentUser)
        {
            var user = await GetUserAsync(currentUser);
           // Console.WriteLine("in get symptoms cu user " + user.Email);
            if (user == null)
                throw new InvalidOperationException("No logged-in user found.");

            var recentSymptoms = await GetRecentSymptomsAsync(3, user.Id);

            return recentSymptoms.Select(s => s.SymptomName).ToList();
        }
    }
}
