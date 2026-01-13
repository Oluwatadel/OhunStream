
namespace OhunStream.Application.Repositories
{
    public interface ISesionRepository
    {
        Task<Session> CreateAsync(Session session, CancellationToken cancellationToken = default);
        Task<Session> GetAsync (Guid sessionId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Session>> GetAllAsync (CancellationToken cancellationToken = default);
    }
}
