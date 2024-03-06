using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using net_iot_data.Data.Entities;

namespace net_iot_data.Data.Configs
{
    public class HealthCheckEntityConfig : IEntityTypeConfiguration<HealthCheckEntity>
    {
        public void Configure(EntityTypeBuilder<HealthCheckEntity> builder)
        {
            builder.ToTable("health_check")
                   .HasKey(h => h.HealthCheckKey);

            builder.Property(h => h.HealthCheckKey)
                   .HasColumnName("health_check_ky")
                   .IsRequired();

            builder.Property(h => h.Success)
                   .HasColumnName("success")
                   .IsRequired();

            // Audit fields
            builder.Property(h => h.Action)
                   .HasColumnName("action")
                   .HasMaxLength(6);

            builder.Property(h => h.PageId)
                   .HasColumnName("page_id")
                   .HasMaxLength(15);

            builder.Property(h => h.LastUpdatedBy)
                   .HasColumnName("lst_updtd_by")
                   .HasMaxLength(25);

            builder.Property(h => h.EstablishedBy)
                   .HasColumnName("estbd_by")
                   .HasMaxLength(25);

            builder.Property(h => h.EstablishedDateTime)
                   .HasColumnName("estbd_dt_tm")
                   .IsRequired();

            builder.Property(h => h.LastUpdatedDateTime)
                   .HasColumnName("lst_updtd_dt_tm")
                   .IsRequired();
        }
    }
}