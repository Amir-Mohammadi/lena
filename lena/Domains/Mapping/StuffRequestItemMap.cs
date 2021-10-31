using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class StuffRequestItemMap : IEntityTypeConfiguration<StuffRequestItem>
  {
    public void Configure(EntityTypeBuilder<StuffRequestItem> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_StuffRequestItem");
      builder.Property(x => x.Id);
      builder.Property(x => x.StuffId);
      builder.Property(x => x.Qty);
      builder.Property(x => x.ResponsedQty);
      builder.Property(x => x.UnitId);
      builder.Property(x => x.StuffRequestId);
      builder.Property(x => x.Status);
      builder.Property(x => x.BillOfMaterialVersion);
      builder.HasOne(x => x.Stuff).WithMany(x => x.StuffRequestItems).HasForeignKey(x => x.StuffId);
      builder.HasOne(x => x.Unit).WithMany(x => x.StuffRequestItems).HasForeignKey(x => x.UnitId);
      builder.HasOne(x => x.StuffRequest).WithMany(x => x.StuffRequestItems).HasForeignKey(x => x.StuffRequestId);
      builder.HasOne(x => x.BillOfMaterial).WithMany(x => x.StuffRequestItems).HasForeignKey(x => new
      {
        x.BillOfMaterialVersion,
        x.StuffId
      });
    }
  }
}
