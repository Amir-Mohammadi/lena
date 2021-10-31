using System.Reflection;
using core.Autofac;
using core.Extensions;
using Microsoft.EntityFrameworkCore;
namespace core.Data
{
  public class ApplicationDbContext : DbContext, IScopedDependency
  {
    private readonly Assembly assembly;
    public ApplicationDbContext(DbContextOptions options, Assembly assembly = null)
         : base(options)
    {
      this.assembly = assembly ?? Assembly.GetEntryAssembly();
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);
      modelBuilder.RegisterAllEntities(assembly: assembly);
      modelBuilder.RegisterIEntityTypeConfiguration(assembly: assembly);
      modelBuilder.AddRestrictDeleteBehaviorConvention();
      // modelBuilder.AddPluralizingTableNameConvention();
    }
  }
}