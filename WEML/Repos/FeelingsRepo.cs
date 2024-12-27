//using Microsoft.EntityFrameworkCore;
//using WEML.Data;
//using WEML.Models;

//namespace WEML.Repos;

//public class FeelingsRepo
//{
//    private readonly ApplicationDbContext _context;


//    public FeelingsRepo(ApplicationDbContext context)
//    {
//        _context = context;
//    }

//    public async Task AddFeelingAsync(Feeling feeling)
//    {
//        if (feeling == null)
//        {
//            throw new ArgumentNullException(nameof(feeling));
//        }

//        _context.Feelings.Add(feeling);

//        await _context.SaveChangesAsync();
//    }

//    public async Task<List<Feeling>> GetAllFeelingsAsync()
//    {
//        return await _context.Feelings.ToListAsync();
//    }

//    public async Task<IEnumerable<Feeling>> GetFeelingsByDateRangeAsync(DateTime startDate, DateTime endDate)
//    {
//        return await _context.Feelings
//            .Where(f => f.DateTime >= startDate && f.DateTime <= endDate)
//            .ToListAsync();
//    }

//    public async Task<IEnumerable<Feeling>> GetAllFeelingsByDate(DateTime date)
//    {
//        return await _context.Feelings
//            .Where(f => f.DateTime.Date == date.Date)
//            .ToListAsync();
//    }


//}
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WEML.Areas.Identity.Data;
using WEML.Data;
using WEML.Models;

namespace WEML.Repos
{
    public class FeelingsRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public FeelingsRepo(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        private async Task<User?> GetUserAsync(ClaimsPrincipal currentUser)
        {
            return await _userManager.GetUserAsync(currentUser);
        }

        private IQueryable<Feeling> GetUserFeelingsQuery(string userId)
        {
            return from feeling in _context.Feelings
                   join feelingUser in _context.Set<FeelingUser>() on feeling.FeelingId equals feelingUser.FeelingId
                   where feelingUser.UserId == userId.ToString()
                   select feeling;
        }

        public async Task AddFeelingAsync(Feeling feeling, ClaimsPrincipal currentUser)
        {
            var user = await GetUserAsync(currentUser);

            if (user == null)
                throw new InvalidOperationException("No logged-in user found.");

            if (feeling == null)
                throw new ArgumentNullException(nameof(feeling));

            _context.Feelings.Add(feeling);

            if (user.FeelingUsers == null)
                user.FeelingUsers = new List<FeelingUser>();

            user.FeelingUsers.Add(new FeelingUser
            {
                FeelingId = feeling.FeelingId,
                UserId = user.UserId.ToString(),
            });

            await _context.SaveChangesAsync();
        }

            
        public async Task<List<Feeling>> GetAllFeelingsAsync(ClaimsPrincipal currentUser)
        {
            var user = await GetUserAsync(currentUser);

            if (user == null)
                throw new InvalidOperationException("No logged-in user found.");

            return await GetUserFeelingsQuery(user.Id).ToListAsync();
        }

        public async Task<IEnumerable<Feeling>> GetFeelingsByDateRangeAsync(DateTime startDate, DateTime endDate, ClaimsPrincipal currentUser)
        {
            var user = await GetUserAsync(currentUser);

            if (user == null)
                throw new InvalidOperationException("No logged-in user found.");

            return await GetUserFeelingsQuery(user.Id)
                .Where(f => f.DateTime >= startDate && f.DateTime <= endDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Feeling>> GetAllFeelingsByDate(DateTime date, ClaimsPrincipal currentUser)
        {
            var user = await GetUserAsync(currentUser);

            if (user == null)
                throw new InvalidOperationException("No logged-in user found.");

            return await GetUserFeelingsQuery(user.Id)
                .Where(f => f.DateTime.Date == date.Date)
                .ToListAsync();
        }
    }
}
