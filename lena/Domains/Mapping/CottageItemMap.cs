using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class CottageItemMap : IEntityTypeConfiguration<CottageItem>
  {
    public void Configure(EntityTypeBuilder<CottageItem> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("CottageItems");
      builder.Property(x => x.Id);
      builder.HasRowVersion();
      builder.Property(x => x.BankOrderDetailId);
      builder.Property(x => x.StuffHSGroupId);
      builder.HasOne(x => x.Cottage).WithMany(x => x.CottageItems);
      builder.HasOne(x => x.BankOrderDetail).WithMany(x => x.CottageItems).HasForeignKey(x => x.BankOrderDetailId);
      builder.HasOne(x => x.StuffHSGroup).WithMany(x => x.CottageItems).HasForeignKey(x => x.StuffHSGroupId);
    }
  }
}
