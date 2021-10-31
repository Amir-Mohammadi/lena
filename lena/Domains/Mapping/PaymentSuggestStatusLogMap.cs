using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class PaymentSuggestStatusLogMap : IEntityTypeConfiguration<PaymentSuggestStatusLog>
  {
    public void Configure(EntityTypeBuilder<PaymentSuggestStatusLog> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("PaymentSuggestStatusLogs");
      builder.Property(x => x.Id);
      builder.HasRowVersion();
      builder.Property(x => x.RegisterDateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.Description);
      builder.Property(x => x.RegisterarUserId);
      builder.Property(x => x.QualityControlId);
      builder.HasOne(x => x.RegisterarUser).WithMany(x => x.PaymentSuggestStatusLogs).HasForeignKey(x => x.RegisterarUserId);
      builder.HasOne(x => x.QualityControl).WithMany(x => x.PaymentSuggestStatusLogs).HasForeignKey(x => x.QualityControlId);
    }
  }
}
