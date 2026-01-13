using Dispatcher;
using OhunStream.Domain.Aggregate.Enum;

namespace OhunStream.Application.Commands
{
    public class CreateSession
    {
        public record StartSessionRequest(SessionMode SessionMode, CancellationToken cancellationToken = default)
            : IRequest<SessionResponse>;

        public record SessionResponse(Guid Id, SessionMode SessionMode, SessionStatus SessionStatus, Guid HostId, DateTime CreatedAt);
        
        public async Task<SessionResponse> Handle(StartSessionRequest request)
        {
            return await Task.FromResult<SessionResponse>(new SessionResponse(
                Guid.NewGuid(), SessionMode.Interactive, SessionStatus.Live, Guid.NewGuid(), DateTime.UtcNow));
        }
    }
}
