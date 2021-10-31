using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class RepairProductionMap : IEntityTypeConfiguration<RepairProduction>
  {
    public void Configure(EntityTypeBuilder<RepairProduction> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("RepairProductions");
      builder.HasDescription();
      builder.HasSaveLog();
      builder.HasRowVersion();
      builder.Property(x => x.Id);
      builder.Property(x => x.ReturnOfSaleId);
      builder.Property(x => x.Status);
      builder.Property(x => x.WarrantyExpirationExceptionId);
      builder.Property(x => x.ProductionId);
      builder.Property(x => x.ReferenceRepairProductionId);
      builder.Property(x => x.SerialStatus);
      builder.HasOne(x => x.ReturnOfSale).WithMany(x => x.RepairProductions).HasForeignKey(x => x.ReturnOfSaleId);
      builder.HasOne(x => x.Production).WithMany(x => x.RepairProductions).HasForeignKey(x => x.ProductionId);
      builder.HasOne(x => x.BaseRepairProduction).WithOne(x => x.ReferenceRepairProduction).HasForeignKey<RepairProduction>(x => x.ReferenceRepairProductionId);
    }
  }
}