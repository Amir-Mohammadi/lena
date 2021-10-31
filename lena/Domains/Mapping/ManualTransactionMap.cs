using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ManualTransactionMap : IEntityTypeConfiguration<ManualTransaction>
  {
    public void Configure(EntityTypeBuilder<ManualTransaction> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_ManualTransaction");
      builder.Property(x => x.StuffId);
      builder.Property(x => x.ProviderId);
      builder.Property(x => x.BillOfMaterialVersion);
      builder.Property(x => x.Qty);
      builder.Property(x => x.QtyPerBox);
      builder.Property(x => x.WarehouseId);
      builder.Property(x => x.UnitId);
      builder.HasOne(x => x.Provider).WithMany(x => x.ManualTransactions).HasForeignKey(x => x.ProviderId);
      builder.HasOne(x => x.Warehouse).WithMany(x => x.ManualTransactions).HasForeignKey(x => x.WarehouseId);
      builder.HasOne(x => x.Unit).WithMany(x => x.ManualTransactions).HasForeignKey(x => x.UnitId);
      builder.HasOne(x => x.Stuff).WithMany(x => x.ManualTransactions).HasForeignKey(x => x.StuffId);
    }
  }
}
