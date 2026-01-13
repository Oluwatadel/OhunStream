using Microsoft.EntityFrameworkCore;
using OhunStream.Application.Repositories;
using OhunStream.Infrastructure.Persistence.Persistence;

namespace OhunStream.Infrastructure.Persistence.Repositories
{
    public class SessionRepository : ISesionRepository
    {
        private readonly OhunStreamDbContext _context;

        public SessionRepository(OhunStreamDbContext context)
        {
            _context = context;
        }

        public async Task<Session> CreateAsync(Session session, CancellationToken cancellationToken = default)
        {
            await _context.Sessions.AddAsync(session, cancellationToken);
            return session;
        }

        public async Task<IEnumerable<Session>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Sessions.ToListAsync(cancellationToken);
        }

        public async Task<Session> GetAsync(Guid sessionId, CancellationToken cancellationToken = default)
        {
            return await _context.Sessions.FirstOrDefaultAsync(a => a.Id == sessionId, cancellationToken); 
        }
    }
}
