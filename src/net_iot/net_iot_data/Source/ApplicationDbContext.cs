using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using net_iot_core.Services.Monitoring.Data.Configs;
using net_iot_data.Data.Configs;
using net_iot_data.Data.Entities;
using net_iot_data.Options;

namespace net_iot_data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly DatabaseOptions _databaseOptions;

        #region DbSets

        // add more db sets here
        public DbSet<HealthCheckEntity> HealthChecks { get; set; }
        public DbSet<MessageLogEntity> Logs { get; set; }

        #endregion

        public ApplicationDbContext(IOptions<DatabaseOptions> databaseOptions)
        {
            _databaseOptions = databaseOptions.Value;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_databaseOptions.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // add more entity configurations here
            modelBuilder.ApplyConfiguration(new HealthCheckEntityConfig());
            modelBuilder.ApplyConfiguration(new MessageLogEntityConfig());
        }

        public void Create()
        {
            this.Database.EnsureCreated();
        }
    }
}
