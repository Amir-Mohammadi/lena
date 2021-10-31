using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class CargoCostMap : IEntityTypeConfiguration<CargoCost>
  {
    public void Configure(EntityTypeBuilder<CargoCost> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("CargoCosts");
      builder.Property(x => x.Id);
      builder.Property(x => x.FinancialDocumentCostId);
      builder.Property(x => x.Amount);
      builder.Property(x => x.CargoId);
      builder.Property(x => x.CargoItemId);
      builder.HasRowVersion();
      builder.HasOne(x => x.FinancialDocumentCost).WithMany(x => x.CargoCosts).HasForeignKey(x => x.FinancialDocumentCostId);
      builder.HasOne(x => x.Cargo).WithMany(x => x.CargoCosts).HasForeignKey(x => x.CargoId);
      builder.HasOne(x => x.CargoItem).WithMany(x => x.CargoCosts).HasForeignKey(x => x.CargoItemId);
    }
  }
}
