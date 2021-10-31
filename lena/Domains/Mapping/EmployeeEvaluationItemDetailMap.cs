using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class EmployeeEvaluationItemDetailMap : IEntityTypeConfiguration<EmployeeEvaluationItemDetail>
  {
    public void Configure(EntityTypeBuilder<EmployeeEvaluationItemDetail> builder)
    {
      builder.HasKey(x => new
      {
        x.EmployeeEvaluationId,
        x.EvaluationCategoryId,
        x.EvaluationCategoryItemId
      });
      builder.ToTable("EmployeeEvaluationItemDetails");
      builder.Property(x => x.EmployeeEvaluationId);
      builder.Property(x => x.EvaluationCategoryId);
      builder.Property(x => x.EvaluationCategoryItemId);
      builder.Property(x => x.Score).IsRequired();
      builder.HasRowVersion();
      builder.HasOne(x => x.EmployeeEvaluationItem).WithMany(x => x.EmployeeEvaluationItemDetails).HasForeignKey(x => new { x.EmployeeEvaluationId, x.EvaluationCategoryId });
      builder.HasOne(x => x.EvaluationCategoryItem).WithMany(x => x.EmployeeEvaluationItemDetails).HasForeignKey(x => x.EvaluationCategoryItemId);
    }
  }
}