using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class BillOfMaterialPublishRequestMap : IEntityTypeConfiguration<BillOfMaterialPublishRequest>
  {
    public void Configure(EntityTypeBuilder<BillOfMaterialPublishRequest> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_BillOfMaterialPublishRequest");
      builder.Property(x => x.Id);
      builder.Property(x => x.BillOfMaterialVersion);
      builder.Property(x => x.BillOfMaterialStuffId);
      builder.Property(x => x.Status);
      builder.Property(x => x.Type);
      builder.HasOne(x => x.BillOfMaterial).WithMany(x => x.BillOfMaterialPublishRequests).HasForeignKey(x => new
      {
        x.BillOfMaterialVersion,
        x.BillOfMaterialStuffId
      });
    }
  }
}
