using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using core.Data;
using System.Reflection;
namespace lena
{
  public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
  {
    public ApplicationDbContext CreateDbContext(string[] args)
    {
      var basePath = Directory.GetCurrentDirectory();
      var configuration = new ConfigurationBuilder()
          .SetBasePath(basePath)
          .AddJsonFile("appsettings.json")
          .Build();
      var connectionString = configuration.GetConnectionString("SqlServer");
      var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
      builder.UseSqlServer(connectionString,
                           x => x.MigrationsAssembly("lena"));
      return new ApplicationDbContext(builder.Options, Assembly.GetExecutingAssembly());
    }
  }
}