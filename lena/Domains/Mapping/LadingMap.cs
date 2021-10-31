using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class LadingMap : IEntityTypeConfiguration<Lading>
  {
    public void Configure(EntityTypeBuilder<Lading> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_Lading");
      builder.Property(x => x.Id);
      builder.Property(x => x.Type);
      builder.Property(x => x.BankOrderId);
      builder.Property(x => x.CustomsValue);
      builder.Property(x => x.ActualWeight);
      builder.Property(x => x.BoxCount);
      builder.Property(x => x.KotazhCode);
      builder.Property(x => x.SataCode);
      builder.Property(x => x.DeliveryDateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.TransportDateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.CustomhouseId);
      builder.Property(x => x.IsLocked);
      builder.Property(x => x.HasReceiptLicence);
      builder.Property(x => x.ReceiptLicenceDateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.HasLadingChangeRequest);
      builder.Property(x => x.LadingBlockerId);
      builder.Property(x => x.NeedToCost);
      builder.HasOne(x => x.LadingBlocker).WithMany(x => x.Ladings).HasForeignKey(x => x.LadingBlockerId);
      builder.HasOne(x => x.BankOrder).WithMany(x => x.Ladings).HasForeignKey(x => x.BankOrderId);
      builder.HasOne(x => x.Customhouse).WithMany(x => x.Ladings).HasForeignKey(x => x.CustomhouseId);
      builder.HasOne(x => x.BankOrderCurrencySource).WithOne(x => x.Lading).HasForeignKey<BankOrderCurrencySource>(x => x.LadingId);
      builder.HasOne(x => x.City).WithMany(x => x.Ladings).HasForeignKey(x => x.CityId);
    }
  }
}