using Dispatcher;
using FluentValidation;
using OhunStream.Domain.Aggregate.Enum;

namespace OhunStream.Application.Commands
{
    public class StartSessionCommand
    {
        public record StartSessionRequest(SessionMode SessionMode)
            : IRequest<StartSessionResponse>;

        public record StartSessionResponse(Guid Id, SessionMode SessionMode, SessionStatus SessionStatus, Guid HostId, DateTime CreatedAt);
        
        public class CreateSessionCommandValidator : AbstractValidator<StartSessionRequest>
        {
            public CreateSessionCommandValidator()
            {
                RuleFor(cr => cr.SessionMode)
                    .NotEmpty()
                    .NotNull()
                    .WithMessage("SessionMode is requide");
            }
        }

        public async Task<StartSessionResponse> Handle(StartSessionRequest request)
        {
            return await Task.FromResult<StartSessionResponse>(new StartSessionResponse(
                Guid.NewGuid(), SessionMode.Interactive, SessionStatus.Live, Guid.NewGuid(), DateTime.UtcNow));
        }
    }
}
