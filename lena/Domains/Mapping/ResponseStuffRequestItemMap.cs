using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ResponseStuffRequestItemMap : IEntityTypeConfiguration<ResponseStuffRequestItem>
  {
    public void Configure(EntityTypeBuilder<ResponseStuffRequestItem> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_ResponseStuffRequestItem");
      builder.Property(x => x.Id);
      builder.Property(x => x.StuffRequestItemId);
      builder.Property(x => x.Qty);
      builder.Property(x => x.Status);
      builder.Property(x => x.StuffId);
      builder.Property(x => x.BillOfMaterialVersion);
      builder.HasOne(x => x.StuffRequestItem).WithMany(x => x.ResponseStuffRequestItems).HasForeignKey(x => x.StuffRequestItemId);
      builder.HasOne(x => x.Stuff).WithMany(x => x.ResponseStuffRequestItems).HasForeignKey(x => x.StuffId);
      builder.HasOne(x => x.BillOfMaterial).WithMany(x => x.ResponseStuffRequestItems).HasForeignKey(x => new
      {
        x.BillOfMaterialVersion,
        x.StuffId
      });
      builder.HasOne(x => x.RequestWarehouseIssue).WithMany(x => x.ResponseStuffRequestItems).HasForeignKey(x => x.RequestWarehouseIssueId);
    }
  }
}
