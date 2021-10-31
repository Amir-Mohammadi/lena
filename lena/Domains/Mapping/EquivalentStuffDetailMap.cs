using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class EquivalentStuffDetailMap : IEntityTypeConfiguration<EquivalentStuffDetail>
  {
    public void Configure(EntityTypeBuilder<EquivalentStuffDetail> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("EquivalentStuffDetails");
      builder.Property(x => x.Id);
      builder.Property(x => x.Value);
      builder.Property(x => x.UnitId);
      builder.Property(x => x.ForQty);
      builder.Property(x => x.StuffId);
      builder.Property(x => x.SemiProductBillOfMaterialVersion);
      builder.Property(x => x.EquivalentStuffId);
      builder.HasRowVersion();
      builder.HasOne(x => x.EquivalentStuff).WithMany(x => x.EquivalentStuffDetails).HasForeignKey(x => x.EquivalentStuffId);
      builder.HasOne(x => x.Stuff).WithMany(x => x.EquivalentStuffDetails).HasForeignKey(x => x.StuffId);
      builder.HasOne(x => x.Unit).WithMany(x => x.EquivalentStuffDetails).HasForeignKey(x => x.UnitId);
      builder.HasOne(x => x.SemiProductBillOfMaterial).WithMany(x => x.UsedInEquivalentStuffs).HasForeignKey(x => new
      {
        x.SemiProductBillOfMaterialVersion,
        x.StuffId
      });
    }
  }
}
