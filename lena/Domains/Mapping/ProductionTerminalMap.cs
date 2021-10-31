using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ProductionTerminalMap : IEntityTypeConfiguration<ProductionTerminal>
  {
    public void Configure(EntityTypeBuilder<ProductionTerminal> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("ProductionTerminals");
      builder.Property(x => x.Id);//.HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
      builder.Property(x => x.Description);
      builder.Property(x => x.ProductionLineId);
      builder.Property(x => x.NetworkId);
      builder.Property(x => x.IsActive);
      builder.Property(x => x.Type);
      builder.Property(x => x.EmployeeId);
      builder.HasRowVersion();
      builder.HasOne(x => x.ProductionLine).WithMany(x => x.ProductionTerminals).HasForeignKey(x => x.ProductionLineId);
      builder.HasOne(x => x.Employee).WithOne(x => x.ProductionTerminal).HasForeignKey<ProductionTerminal>(x => x.EmployeeId);
    }
  }
}