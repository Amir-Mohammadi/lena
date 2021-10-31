using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class EvaluationCategoryItemMap : IEntityTypeConfiguration<EvaluationCategoryItem>
  {
    public void Configure(EntityTypeBuilder<EvaluationCategoryItem> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("EvaluationCategoryItems");
      builder.Property(x => x.Id);
      builder.Property(x => x.EvaluationCategoryId).IsRequired();
      builder.Property(x => x.Question).IsRequired();
      builder.Property(x => x.IsArchive).IsRequired();
      builder.HasRowVersion();
      builder.HasOne(x => x.EvaluationCategory).WithMany(x => x.EvaluationCategoryItems).HasForeignKey(x => x.EvaluationCategoryId);
    }
  }
}
