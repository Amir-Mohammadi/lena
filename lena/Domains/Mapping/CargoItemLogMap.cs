using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class CargoItemLogMap : IEntityTypeConfiguration<CargoItemLog>
  {
    public void Configure(EntityTypeBuilder<CargoItemLog> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("CargoItemLogs");
      builder.Property(x => x.Id);
      builder.Property(x => x.ModifierUserId);
      builder.Property(x => x.ModifyDateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.CargoItemId);
      builder.Property(x => x.CargoItemCode);
      builder.Property(x => x.IsDelete);
      builder.Property(x => x.NewCargoItemQty);
      builder.Property(x => x.OldCargoItemDetailQty);
      builder.Property(x => x.CargoItemLogStatus);
      builder.HasRowVersion();
      builder.HasOne(x => x.ModifierUser).WithMany(x => x.CargoItemLogs).HasForeignKey(x => x.ModifierUserId);
      builder.HasOne(x => x.CargoItem).WithMany(x => x.CargoItemLogs).HasForeignKey(x => x.CargoItemId);
    }
  }
}
