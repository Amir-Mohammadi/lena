using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class CostCenterMap : IEntityTypeConfiguration<CostCenter>
  {
    public void Configure(EntityTypeBuilder<CostCenter> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("CostCenters");
      builder.Property(x => x.Id);
      builder.Property(x => x.Name).IsRequired();
      builder.Property(x => x.Description);
      builder.Property(x => x.Status);
      builder.Property(x => x.ConfirmDateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.ConfirmerUserId);
      builder.Property(x => x.RowVersion).IsRowVersion().IsRequired();
      builder.HasOne(x => x.ConfirmerUser).WithMany(x => x.ConfirmerCostCenters).HasForeignKey(x => x.ConfirmerUserId);
    }
  }
}
