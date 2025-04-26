using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Security.AccessControl;
using SalesPredictionProject.Models.Entity;

namespace SalesPredictionProject.Data
{
    public class MyDbContext : DbContext
    {

        public DbSet<SalesDataRow> SalesDataRows { get; set; }
        //public DbSet<ForecastResult> ForecastResults { get; set; }
        public MyDbContext(DbContextOptions options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // This would only be necessary if you're not passing options via DI
                IConfigurationRoot configuration = new ConfigurationBuilder()
                                          .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                                          .AddJsonFile("appsettings.json")
                                          .Build();
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlServer(connectionString, sqlServerOptionsAction =>
                {
                    sqlServerOptionsAction.EnableRetryOnFailure();
                });
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SalesDataRow>()
                .HasNoKey()
                .Property(s => s.SalesAmount)
                .HasColumnType("decimal(18,2)"); // Precision 18, Scale 2
        }

    }
}
