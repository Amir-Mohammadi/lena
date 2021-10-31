using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class TagCountingMap : IEntityTypeConfiguration<TagCounting>
  {
    public void Configure(EntityTypeBuilder<TagCounting> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("TagCountings");
      builder.Property(x => x.Id);
      builder.Property(x => x.StockCheckingTagId);
      builder.Property(x => x.Amount);
      builder.Property(x => x.UnitId);
      builder.Property(x => x.DateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.UserId);
      builder.Property(x => x.IsDelete);
      builder.HasRowVersion();
      builder.HasOne(x => x.StockCheckingTag).WithMany(x => x.TagCountings).HasForeignKey(x => x.StockCheckingTagId);
      builder.HasOne(x => x.Unit).WithMany(x => x.TagCountings).HasForeignKey(x => x.UnitId);
      builder.HasOne(x => x.User).WithMany(x => x.TagCountings).HasForeignKey(x => x.UserId);
    }
  }
}
