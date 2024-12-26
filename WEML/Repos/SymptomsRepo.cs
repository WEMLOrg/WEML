//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using WEML.Areas.Identity.Data;
//using WEML.Data;
//using WEML.Models;

//namespace WEML.Repos;

//public class SymptomsRepo
//{
//    private readonly ApplicationDbContext _context;

//    public SymptomsRepo(ApplicationDbContext context)
//    {
//        _context = context;
//    }

//    public async Task AddSymptomAsync(Symptom symptom)
//    {
//       
//        if (symptom == null)
//        {
//            throw new ArgumentNullException(nameof(symptom));
//        }

//        _context.Symptoms.Add(symptom);
//        await _context.SaveChangesAsync();
//    }

//    public async Task<List<Symptom>> GetAllSymptomsAsync()
//    {
//        return await _context.Symptoms.ToListAsync();
//    }

//    public async Task<IEnumerable<Symptom>> GetSymptomsByDateRangeAsync(DateTime startDate, DateTime endDate)
//    {
//        return await _context.Symptoms
//            .Where(s => s.DateTime >= startDate && s.DateTime <= endDate)
//            .ToListAsync();
//    }

//    public async Task<IEnumerable<Symptom>> GetAllSymptomsByDate(DateTime date)
//    {
//        return await _context.Symptoms
//            .Where(s => s.DateTime.Date == date.Date)
//            .ToListAsync();
//    }

//    private async Task<List<Symptom>> GetRecentSymptomsAsync(int count = 10)
//    {
//        return await _context.Symptoms
//            .OrderByDescending(s => s.DateTime) 
//            .Take(count)                        
//            .ToListAsync();
//    }

//    public async Task<List<string>> GetMostRecentSymptomsAsync()
//    {
//        var recentSymptoms = await GetRecentSymptomsAsync(10);

//        return recentSymptoms.Select(s => s.SymptomName).ToList();
//    }


//}
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

        public SymptomsRepo(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private async Task<User?> GetUserAsync(ClaimsPrincipal currentUser)
        {
            return await _userManager.GetUserAsync(currentUser);
        }

        private IQueryable<Symptom> GetUserSymptomsQuery(Guid userId)
        {
            return from symptom in _context.Symptoms
                   join symptomUser in _context.Set<SymptomUser>() on symptom.SymptomId equals symptomUser.SymptomId
                   where symptomUser.UserId == userId.ToString()
                   select symptom;
        }

        public async Task AddSymptomAsync(Symptom symptom, ClaimsPrincipal currentUser)
        {
            var user = await GetUserAsync(currentUser);

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

            var userSymptomCount = await GetUserSymptomsQuery(user.UserId).CountAsync();
            if (userSymptomCount % 10 == 0)
            {
                var userSymptoms = await GetUserSymptomsQuery(user.UserId).ToListAsync();
                SendStatusService sendStatus = new SendStatusService(user, userSymptoms);
            }
        }

        public async Task<List<Symptom>> GetAllSymptomsAsync(ClaimsPrincipal currentUser)
        {
            var user = await GetUserAsync(currentUser);

            if (user == null)
                throw new InvalidOperationException("No logged-in user found.");

            return await GetUserSymptomsQuery(user.UserId).ToListAsync();
        }

        public async Task<IEnumerable<Symptom>> GetSymptomsByDateRangeAsync(DateTime startDate, DateTime endDate, ClaimsPrincipal currentUser)
        {
            var user = await GetUserAsync(currentUser);

            if (user == null)
                throw new InvalidOperationException("No logged-in user found.");

            return await GetUserSymptomsQuery(user.UserId)
                .Where(s => s.DateTime >= startDate && s.DateTime <= endDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Symptom>> GetAllSymptomsByDate(DateTime date, ClaimsPrincipal currentUser)
        {
            var user = await GetUserAsync(currentUser);

            if (user == null)
                throw new InvalidOperationException("No logged-in user found.");

            return await GetUserSymptomsQuery(user.UserId)
                .Where(s => s.DateTime.Date == date.Date)
                .ToListAsync();
        }

        private async Task<List<Symptom>> GetRecentSymptomsAsync(int count, Guid userId)
        {
            return await GetUserSymptomsQuery(userId)
                .OrderByDescending(s => s.DateTime)
                .Take(count)
                .ToListAsync();
        }

        public async Task<List<string>> GetMostRecentSymptomsAsync(ClaimsPrincipal currentUser)
        {
            var user = await GetUserAsync(currentUser);

            if (user == null)
                throw new InvalidOperationException("No logged-in user found.");

            var recentSymptoms = await GetRecentSymptomsAsync(10, user.UserId);

            return recentSymptoms.Select(s => s.SymptomName).ToList();
        }
    }
}
