using Microsoft.EntityFrameworkCore;
using OhunStream.Application.Repositories;
using OhunStream.Domain.Aggregate;

namespace OhunStream.Infrastructure.Persistence.Persistence
{
    public class OhunStreamDbContext(DbContextOptions<OhunStreamDbContext> options) : DbContext(options)
    {
        public DbSet<Session> Sessions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
