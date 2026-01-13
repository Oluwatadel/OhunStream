using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OhunStream.Domain.Aggregate;
using OhunStream.Domain.Aggregate.Enum;

namespace OhunStream.Infrastructure.Persistence.EntityConfiguration
{
    public class SessionConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.ToTable("Sessions");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id)
                     .IsRequired()
                     .HasColumnName("session_id")
                     .HasColumnType("uuid");

            builder.Property(s => s.Mode)
                   .IsRequired()
                   .HasConversion(new EnumToStringConverter<SessionMode>())
                   .HasColumnName("session_mode")
                   .HasColumnType("varchar(20)");


            builder.Property(s => s.Status)
                     .IsRequired()
                     .HasColumnName("session_status")
                     .HasConversion(new EnumToStringConverter<SessionStatus>())
                     .HasColumnType("varchar(20)");

            builder.Property(s => s.HostId)
                .IsRequired()
                .HasColumnName("host_id")
                .HasColumnType("uuid");

            builder.Property(s => s.CreatedAt)
                     .IsRequired()
                     .HasColumnName("created_at")
                     .HasColumnType("timestamp");
        }
    }
}
