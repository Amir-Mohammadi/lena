using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class StuffDocumentMap : IEntityTypeConfiguration<StuffDocument>
  {
    public void Configure(EntityTypeBuilder<StuffDocument> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("StuffDocuments");
      builder.Property(x => x.Id);
      builder.Property(x => x.Title).IsRequired();
      builder.Property(x => x.Description);
      builder.Property(x => x.DocumentId);
      builder.Property(x => x.StuffId);
      builder.Property(x => x.StuffDocumentType);
      builder.Property(x => x.UserId);
      builder.Property(x => x.DateTime).HasColumnType("smalldatetime");
      builder.HasRowVersion();
      builder.HasOne(x => x.Stuff).WithMany(x => x.StuffDocuments).HasForeignKey(x => x.StuffId);
      builder.HasOne(x => x.User).WithMany(x => x.StuffDocuments).HasForeignKey(x => x.UserId);
    }
  }
}
