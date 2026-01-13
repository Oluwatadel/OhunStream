using OhunStream.Domain.Aggregate.Enum;

namespace OhunStream.Domain.Aggregate
{
    public class Session
    {
        public Guid Id { get; private set; }
        public SessionMode Mode { get; private set; }
        public SessionStatus Status { get; private set; }
        public Guid HostId { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private Session() { }

        private Session(Guid id, Guid hostId, SessionMode mode)
        {
            Id = id;
            HostId = hostId;
            Mode = mode;
            Status = SessionStatus.Created;
            CreatedAt = DateTime.UtcNow;
        }

        public static Session Create(Guid hostId, SessionMode mode)
        {
            if (hostId == Guid.Empty)
                throw new DomainValidationException("Host is required");

            return new Session(Guid.NewGuid(), hostId, mode);
        }

        public void Start()
        {
            if (Status != SessionStatus.Created)
                throw new DomainException("Only a created session can be started");

            Status = SessionStatus.Live;
        }

        public void End()
        {
            if (Status != SessionStatus.Live)
                throw new DomainException("Only a live session can be ended");

            Status = SessionStatus.Ended;
        }

        public bool IsBroadcast()
        => Mode == SessionMode.Broadcast;

        public bool IsInteractive()
            => Mode == SessionMode.Interactive;
    }
}
