using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class GiveBackExitReceiptRequestMap : IEntityTypeConfiguration<GiveBackExitReceiptRequest>
  {
    public void Configure(EntityTypeBuilder<GiveBackExitReceiptRequest> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_GiveBackExitReceiptRequest");
      builder.Property(x => x.Id);
      builder.Property(x => x.QualityControlId);
      builder.HasOne(x => x.QualityControl).WithMany(x => x.GiveBackExitReceiptRequests).HasForeignKey(x => x.QualityControlId);
    }
  }
}
