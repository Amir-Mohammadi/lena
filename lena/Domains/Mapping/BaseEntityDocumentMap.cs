using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class BaseEntityDocumentMap : IEntityTypeConfiguration<BaseEntityDocument>
  {
    public void Configure(EntityTypeBuilder<BaseEntityDocument> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntityDocuments");
      builder.Property(x => x.Id);
      builder.Property(x => x.Description);
      builder.Property(x => x.IsDelete);
      builder.Property(x => x.DateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.BaseEntityId);
      builder.Property(x => x.UserId);
      builder.Property(x => x.DocumentId);
      builder.Property(x => x.BaseEntityDocumentTypeId);
      builder.Property(x => x.CooperatorId);
      builder.HasRowVersion();
      builder.HasOne(x => x.BaseEntity).WithMany(x => x.BaseEntityDocuments).HasForeignKey(x => x.BaseEntityId);
      builder.HasOne(x => x.User).WithMany(x => x.BaseEntityDocuments).HasForeignKey(x => x.UserId);
      builder.HasOne(x => x.BaseEntityDocumentType).WithMany(x => x.BaseEntityDocuments).HasForeignKey(x => x.BaseEntityDocumentTypeId);
      builder.HasOne(x => x.Cooperator).WithMany(x => x.BaseEntityDocuments).HasForeignKey(x => x.CooperatorId);
    }
  }
}
