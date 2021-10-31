using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class WorkPlanMap : IEntityTypeConfiguration<WorkPlan>
  {
    public void Configure(EntityTypeBuilder<WorkPlan> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("WorkPlans");
      builder.Property(x => x.Id);
      builder.Property(x => x.Version);
      builder.Property(x => x.Title);
      builder.Property(x => x.BillOfMaterialStuffId);
      builder.Property(x => x.BillOfMaterialVersion);
      builder.Property(x => x.IsActive);
      builder.Property(x => x.CreateDate).HasColumnType("smalldatetime");
      builder.Property(x => x.Description);
      builder.Property(x => x.IsPublished).IsRequired();
      builder.HasRowVersion();
      builder.HasOne(x => x.BillOfMaterial).WithMany(x => x.WorkPlans).HasForeignKey(x => new
      {
        x.BillOfMaterialVersion,
        x.BillOfMaterialStuffId
      });
    }
  }
}
