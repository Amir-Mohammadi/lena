using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class PriceInquiryMap : IEntityTypeConfiguration<PriceInquiry>
  {
    public void Configure(EntityTypeBuilder<PriceInquiry> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("PriceInquiries");
      builder.Property(x => x.Id);
      builder.Property(x => x.StuffId);
      builder.Property(x => x.CooperatorId);
      builder.Property(x => x.CurrencyId);
      builder.Property(x => x.UserId);
      builder.Property(x => x.Number);
      builder.Property(x => x.Price);
      builder.Property(x => x.PriceAnnunciationDateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.CreateDateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.Description);
      builder.HasRowVersion();
      builder.HasOne(x => x.Stuff).WithMany(x => x.PriceInquries).HasForeignKey(x => x.StuffId);
      builder.HasOne(x => x.Cooperator).WithMany(x => x.PriceInquries).HasForeignKey(x => x.CooperatorId);
      builder.HasOne(x => x.Currency).WithMany(x => x.PriceInquries).HasForeignKey(x => x.CurrencyId);
      builder.HasOne(x => x.User).WithMany(x => x.PriceInquries).HasForeignKey(x => x.UserId);
    }
  }
}
