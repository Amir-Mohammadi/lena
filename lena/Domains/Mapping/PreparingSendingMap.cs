using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class PreparingSendingMap : IEntityTypeConfiguration<PreparingSending>
  {
    public void Configure(EntityTypeBuilder<PreparingSending> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_PreparingSending");
      builder.Property(x => x.Id);
      builder.Property(x => x.SendPermissionId);
      builder.Property(x => x.Status);
      builder.Property(x => x.Qty);
      builder.Property(x => x.UnitId);
      builder.HasOne(x => x.SendPermission).WithMany(x => x.PreparingSendings).HasForeignKey(x => x.SendPermissionId);
      builder.HasOne(x => x.SendProduct).WithOne(x => x.PreparingSending).HasForeignKey<SendProduct>(x => x.PreparingSendingId);
      builder.HasOne(x => x.Unit).WithMany(x => x.PreparingSendings).HasForeignKey(x => x.UnitId);
    }
  }
}
