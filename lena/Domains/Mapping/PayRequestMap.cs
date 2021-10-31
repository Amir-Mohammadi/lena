using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class PayRequestMap : IEntityTypeConfiguration<PayRequest>
  {
    public void Configure(EntityTypeBuilder<PayRequest> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("PayRequests");
      builder.HasRemoveable();
      builder.HasRowVersion();
      builder.HasDescription();
      builder.HasFinancialTransaction();
      builder.HasSaveLog();
      builder.Property(x => x.Id);
      builder.Property(x => x.Status);
      builder.Property(x => x.PayedAmount);
      builder.Property(x => x.DiscountedTotalPrice);
      builder.Property(x => x.DocumentId);
      builder.Property(x => x.QualityControlId);
      builder.HasOne(x => x.QualityControl).WithOne(x => x.PayRequest).HasForeignKey<PayRequest>(x => x.QualityControlId);
    }
  }
}