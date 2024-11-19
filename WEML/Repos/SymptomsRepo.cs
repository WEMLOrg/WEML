using Microsoft.EntityFrameworkCore;
using WEML.Data;
using WEML.Models;

namespace WEML.Repos;

public class SymptomsRepo
{
    private readonly ApplicationDbContext _context;


    public SymptomsRepo(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task AddSymptomAsync(Symptom symptom)
    {
        if (symptom == null)
        {
            throw new ArgumentNullException(nameof(symptom));
        }

        _context.Symptoms.Add(symptom);

        await _context.SaveChangesAsync();
    }
    
    public async Task<List<Symptom>> GetAllSymptomsAsync()
    {
        return await _context.Symptoms.ToListAsync();
    }

    public async Task<IEnumerable<Symptom>> GetSymptomsByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.Symptoms
            .Where(s => s.DateTime >= startDate && s.DateTime <= endDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Symptom>> GetAllSymptomsByDate(DateTime date)
    {
        return await _context.Symptoms
            .Where(s => s.DateTime.Date == date.Date)
            .ToListAsync();
    }
}