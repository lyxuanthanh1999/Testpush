using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.EF
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<EKhoaHocDbContext>
    {
        public EKhoaHocDbContext CreateDbContext(string[] args)
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environmentName}.json")
                .Build();
            var optionsBuilder = new DbContextOptionsBuilder<EKhoaHocDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);
            return new EKhoaHocDbContext(optionsBuilder.Options);
        }
    }
}