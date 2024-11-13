using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using WEML.Models;

namespace WEML.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
           
        }
        public DbSet<Symptom> Symptoms { get; set; }
        public DbSet<Feeling> Feelings { get; set; }
       // public DbSet<User> Users {  get; set; }
    }
}
