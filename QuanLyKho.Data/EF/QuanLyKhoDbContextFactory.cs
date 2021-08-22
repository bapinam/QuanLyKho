using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using QuanLyKho.Date.EF;
using QuanLyKho.Utilities.Constants;
using System.IO;

namespace QuanLyKho.Data.EF
{
    public class QuanLyKhoDbContextFactory : IDesignTimeDbContextFactory<QuanLyKhoDbContext>
    {
        public QuanLyKhoDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString(SystemConstants.MainConectionString);

            var optionsBuilder = new DbContextOptionsBuilder<QuanLyKhoDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new QuanLyKhoDbContext(optionsBuilder.Options);
        }
    }
}