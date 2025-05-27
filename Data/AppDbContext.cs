using Microsoft.EntityFrameworkCore;

namespace boh_api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options) {}

        public DbSet<Verb> Verbs => Set<Verb>();
        public DbSet<Conjugation> Conjugations => Set<Conjugation>();
    }
}
