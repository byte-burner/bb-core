using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using net_iot_data.Data.Entities;

namespace net_iot_core.Services.Monitoring.Data.Configs
{
    public class MessageLogEntityConfig : IEntityTypeConfiguration<MessageLogEntity>
    {
        public void Configure(EntityTypeBuilder<MessageLogEntity> builder)
        {
            builder.ToTable("message_log")
                     .HasKey(h => h.Id);
       
            builder.Property(h => h.Id)
                     .HasColumnName("Id")
                     .IsRequired();

            builder.Property(h => h.AddedDate)
                   .HasColumnName("date");

            builder.Property(h => h.Level)
                   .HasColumnName("level");

            builder.Property(h => h.Message)
                   .HasColumnName("message");

            builder.Property(h => h.Exception)
                   .HasColumnName("exception");
        }
    }
}