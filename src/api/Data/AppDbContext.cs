using Microsoft.EntityFrameworkCore;
using boh_api.Models;

namespace boh_api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Verb> Verbs => Set<Verb>();
        public DbSet<Conjugation> Conjugations => Set<Conjugation>();
        public DbSet<VerbDefinition> VerbDefinitions => Set<VerbDefinition>();
        public DbSet<VerbPronunciation> VerbPronunciations => Set<VerbPronunciation>();
        public DbSet<RelatedWord> RelatedWords => Set<RelatedWord>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Verb>()
                .HasMany(v => v.Conjugations)
                .WithOne(c => c.ParentVerb)
                .HasForeignKey(c => c.VerbId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Verb>()
                .HasMany(v => v.Definitions)
                .WithOne(d => d.ParentVerb)
                .HasForeignKey(d => d.VerbId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Verb>()
                .HasMany(v => v.Pronunciations)
                .WithOne(p => p.ParentVerb)
                .HasForeignKey(p => p.VerbId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Verb>()
                .HasMany(v => v.RelatedWords)
                .WithOne(r => r.ParentVerb)
                .HasForeignKey(r => r.VerbId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Verb>()
                .HasMany(v => v.Tags)
                .WithMany()
                .UsingEntity(j => j.ToTable("VerbTags"));
        }
    }
}
