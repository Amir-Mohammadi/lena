using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class GeneralStuffRequestDetailMap : IEntityTypeConfiguration<GeneralStuffRequestDetail>
  {
    public void Configure(EntityTypeBuilder<GeneralStuffRequestDetail> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("GeneralStuffRequestDetails");
      builder.Property(x => x.Qty).IsRequired();
      builder.HasRowVersion();
      builder.Property(x => x.Description).IsRequired(false);
      builder.HasOne(x => x.StuffRequest);
      builder.HasOne(x => x.PurchaseRequest);
      builder.HasOne(x => x.AlternativePurchaseRequest);
      builder.HasOne(x => x.GeneralStuffRequest).WithMany(x => x.GeneralStuffRequestDetails).HasForeignKey(x => x.GeneralStuffRequestId);
    }
  }
}