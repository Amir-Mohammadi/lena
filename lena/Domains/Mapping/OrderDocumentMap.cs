using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class OrderDocumentMap : IEntityTypeConfiguration<OrderDocument>
  {
    public void Configure(EntityTypeBuilder<OrderDocument> builder)
    {
      builder.HasKey(x => new { x.OrderId, x.DocumentId });
      builder.ToTable("OrderDocuments");
      builder.HasRowVersion();
      builder.HasDocument();
      builder.HasRemoveable();
      builder.Property(x => x.OrderId).IsRequired();
      builder.Property(x => x.ModifiedDate).IsRequired();
      builder.Property(x => x.UserId).IsRequired();
      builder.Property(x => x.IsDelete).IsRequired();
      builder.Property(x => x.Description).IsRequired(false);
      builder.HasOne(x => x.Order).WithMany(x => x.OrderDocuments).HasForeignKey(x => x.OrderId);
      builder.HasOne(x => x.User).WithMany(x => x.OrderDocuments).HasForeignKey(x => x.UserId);
    }
  }
}