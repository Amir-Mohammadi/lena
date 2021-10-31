using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class DocumentResultMap : IEntityTypeConfiguration<DocumentResult>
  {
    public void Configure(EntityTypeBuilder<DocumentResult> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("DocumentResults");
      builder.Property(x => x.Id);
      builder.Property(x => x.FileStream);
      builder.Property(x => x.Name).IsRequired().HasMaxLength(255);
      builder.Property(x => x.IsDirectory);//.HasDatabaseGeneratedOption(new Nullable<DatabaseGeneratedOption>(DatabaseGeneratedOption.Computed));
      builder.Property(x => x.FilePath);//.HasDatabaseGeneratedOption(new Nullable<DatabaseGeneratedOption>(DatabaseGeneratedOption.Computed)).HasMaxLength(4000);
      builder.Property(x => x.FileType);//.HasDatabaseGeneratedOption(new Nullable<DatabaseGeneratedOption>(DatabaseGeneratedOption.Computed)).HasMaxLength(255);
      builder.Property(x => x.CreationTime).HasColumnType("smalldatetime");
      builder.HasRowVersion();
    }
  }
}