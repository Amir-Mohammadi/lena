using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ProductionPlanDetailLevelMap : IEntityTypeConfiguration<ProductionPlanDetailLevel>
  {
    public void Configure(EntityTypeBuilder<ProductionPlanDetailLevel> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("ProductionPlanDetailLevels");
      builder.Property(x => x.Id);
      builder.Property(x => x.ParentId);
      builder.HasRowVersion();
      builder.HasOne(x => x.Parent).WithMany(x => x.Childs).HasForeignKey(x => x.ParentId);
    }
  }
}
