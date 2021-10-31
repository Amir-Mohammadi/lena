using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class RiskMap : IEntityTypeConfiguration<Risk>
  {
    public void Configure(EntityTypeBuilder<Risk> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("Risks");
      builder.Property(x => x.Id);
      builder.Property(x => x.Title).IsRequired();
      builder.Property(x => x.CreatorUserId).IsRequired();
      builder.Property(x => x.CreateDateTime).HasColumnType("smalldatetime").IsRequired();
      builder.Property(x => x.PurchaseRequestId).IsRequired(false);
      builder.Property(x => x.PurchaseOrderId).IsRequired(false);
      builder.Property(x => x.CargoItemId).IsRequired(false);
      builder.Property(x => x.LatestRiskStatusId);
      builder.Property(x => x.LatestRiskResolveId);
      builder.HasRowVersion();
      builder.HasOne(x => x.PurchaseRequest).WithMany(x => x.Risks).HasForeignKey(x => x.PurchaseRequestId);
      builder.HasOne(x => x.CargoItem).WithMany(x => x.Risks).HasForeignKey(x => x.CargoItemId);
      builder.HasOne(x => x.PurchaseOrder).WithMany(x => x.Risks).HasForeignKey(x => x.PurchaseOrderId);
      builder.HasOne(x => x.LatestRiskStatus).WithOne().HasForeignKey<Risk>(x => x.LatestRiskStatusId);
      builder.HasOne(x => x.LatestRiskResolve).WithOne().HasForeignKey<Risk>(x => x.LatestRiskResolveId);
      builder.HasOne(x => x.CreatorUser).WithMany(x => x.Risks).HasForeignKey(x => x.CreatorUserId);
    }
  }
}