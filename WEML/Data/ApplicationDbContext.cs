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
        public DbSet<Challange> Challanges { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<ChallangeQuestions> ChallangeQuestions { get; set; }
        public DbSet<Symptom> Symptoms { get; set; }
        public DbSet<Feeling> Feelings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ChallangeQuestions>()
                .HasKey(cq => new { cq.cId, cq.questionId });
            modelBuilder.Entity<ChallangeQuestions>()
                .HasOne(cq => cq.Challange)
                .WithMany(c => c.ChallangeQuestions)
                .HasForeignKey(cq => cq.cId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ChallangeQuestions>()
                .HasOne(cq => cq.Question)
                .WithMany(q => q.ChallangeQuestions)
                .HasForeignKey(cq => cq.questionId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Question>()
                .Property(q => q.qId)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Challange>()
                .Property(c => c.cId)
                .ValueGeneratedOnAdd();
        }
       // public DbSet<WEML.Models.Article> Article { get; set; } = default!;
        // public DbSet<User> Users {  get; set; }
    }
}
