using Microsoft.EntityFrameworkCore;
using WEML.Data;
using WEML.Models;

namespace WEML.Repos;

public class FeelingsRepo
{
    private readonly ApplicationDbContext _context;


    public FeelingsRepo(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task AddFeelingAsync(Feeling feeling)
    {
        if (feeling == null)
        {
            throw new ArgumentNullException(nameof(feeling));
        }

        _context.Feelings.Add(feeling);

        await _context.SaveChangesAsync();
    }
    
    public async Task<List<Feeling>> GetAllFeelingsAsync()
    {
        return await _context.Feelings.ToListAsync();
    }

    public async Task<IEnumerable<Feeling>> GetFeelingsByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.Feelings
            .Where(f => f.DateTime >= startDate && f.DateTime <= endDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Feeling>> GetAllFeelingsByDate(DateTime date)
    {
        return await _context.Feelings
            .Where(f => f.DateTime.Date == date.Date)
            .ToListAsync();
    }


}