using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class BankOrderDetailMap : IEntityTypeConfiguration<BankOrderDetail>
  {
    public void Configure(EntityTypeBuilder<BankOrderDetail> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("BankOrderDetails");
      builder.Property(x => x.Id);
      builder.Property(x => x.Index);
      builder.Property(x => x.BankOrderId);
      builder.Property(x => x.Qty);
      builder.Property(x => x.UnitId);
      builder.Property(x => x.Fee);
      builder.Property(x => x.StuffHSGroupId);
      builder.Property(x => x.Price);
      builder.Property(x => x.Weight);
      builder.Property(x => x.GrossWeight);
      builder.Property(x => x.Description);
      builder.HasRowVersion();
      builder.HasOne(x => x.Unit).WithMany(x => x.BankOrderDetails).HasForeignKey(x => x.UnitId);
      builder.HasOne(x => x.BankOrder).WithMany(x => x.BankOrderDetails).HasForeignKey(x => x.BankOrderId);
      builder.HasOne(x => x.StuffHSGroup).WithMany(x => x.BankOrderDetails).HasForeignKey(x => x.StuffHSGroupId);
    }
  }
}
