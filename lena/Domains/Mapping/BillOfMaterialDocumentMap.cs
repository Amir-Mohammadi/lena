using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class BillOfMaterialDocumentMap : IEntityTypeConfiguration<BillOfMaterialDocument>
  {
    public void Configure(EntityTypeBuilder<BillOfMaterialDocument> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("BillOfMaterialDocuments");
      builder.Property(x => x.Id);
      builder.Property(x => x.BillOfMaterialVersion);
      builder.Property(x => x.BillOfMaterialStuffId);
      builder.Property(x => x.BillOfMaterialDocumentTypeId);
      builder.Property(x => x.Title).IsRequired();
      builder.Property(x => x.Description);
      builder.Property(x => x.DocumentId);
      builder.HasRowVersion();
      builder.Property(x => x.UserId);
      builder.Property(x => x.IsDelete);
      builder.Property(x => x.DateOfDelete);
      builder.Property(x => x.DeleteUserId);
      builder.HasOne(x => x.BillOfMaterial).WithMany(x => x.BillOfMaterialDocuments).HasForeignKey(x => new
      {
        x.BillOfMaterialVersion,
        x.BillOfMaterialStuffId
      });
      builder.HasOne(x => x.BillOfMaterialDocumentType).WithMany(x => x.BillOfMaterialDocuments).HasForeignKey(x => x.BillOfMaterialDocumentTypeId);
      builder.HasOne(x => x.User).WithMany(x => x.BillOfMaterialDocuments).HasForeignKey(x => x.UserId);
    }
  }
}
