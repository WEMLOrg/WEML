using WEML.Areas.Identity.Data;
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
        public DbSet<SymptomUser> SymptomUsers { get; set; }
        public DbSet<FeelingUser> FeelingUsers { get; set; }

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

            modelBuilder.Entity<Symptom>().Property(s => s.SymptomId);
            modelBuilder.Entity<Feeling>().Property(f => f.FeelingId);

            modelBuilder.Entity<SymptomUser>()
                .HasKey(su => new { su.SymptomId, su.UserId });

            modelBuilder.Entity<SymptomUser>()
                .HasOne(su => su.Symptom)
                .WithMany(s => s.SymptomUsers)
                .HasForeignKey(su => su.SymptomId);

            modelBuilder.Entity<SymptomUser>()
                .HasOne(su => su.User)
                .WithMany(u => u.SymptomUsers)
                .HasForeignKey(su => su.UserId);

            // Correctly define the composite key for FeelingUser
            modelBuilder.Entity<FeelingUser>()
                .HasKey(fu => new { fu.FeelingId, fu.UserId });

            modelBuilder.Entity<FeelingUser>()
                .HasOne(fu => fu.Feeling)
                .WithMany(f => f.FeelingUser)
                .HasForeignKey(fu => fu.FeelingId)
                .OnDelete(DeleteBehavior.Cascade);  // Adjust delete behavior as needed

            modelBuilder.Entity<FeelingUser>()
                .HasOne(fu => fu.User)
                .WithMany(u => u.FeelingUsers)
                .HasForeignKey(fu => fu.UserId)
                .OnDelete(DeleteBehavior.Cascade);  
        }

    }
}
