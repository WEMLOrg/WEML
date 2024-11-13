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
}