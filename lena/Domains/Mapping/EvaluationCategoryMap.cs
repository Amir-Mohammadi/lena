using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class EvaluationCategoryMap : IEntityTypeConfiguration<EvaluationCategory>
  {
    public void Configure(EntityTypeBuilder<EvaluationCategory> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("EvaluationCategories");
      builder.HasKey(x => x.Id);
      builder.Property(x => x.Title).IsRequired();
      builder.Property(x => x.DepartmentId).IsRequired(false);
      builder.HasRowVersion();
      builder.HasOne(x => x.Department).WithMany(x => x.EvaluationCategories).HasForeignKey(x => x.DepartmentId);
    }
  }
}
