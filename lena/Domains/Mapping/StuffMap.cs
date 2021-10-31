using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class StuffMap : IEntityTypeConfiguration<Stuff>
  {
    public void Configure(EntityTypeBuilder<Stuff> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("Stuffs");
      builder.Property(x => x.Id);
      builder.Property(x => x.Name).IsRequired();
      builder.Property(x => x.Noun).IsRequired();
      builder.Property(x => x.Title).IsRequired();
      builder.Property(x => x.Code).IsRequired();
      builder.Property(x => x.IsActive);
      builder.Property(x => x.Description);
      builder.Property(x => x.StuffCategoryId);
      builder.Property(x => x.UnitTypeId);
      builder.Property(x => x.StuffType);
      builder.Property(x => x.StockSafety);
      builder.Property(x => x.FaultyPercentage);
      builder.Property(x => x.NeedToQualityControl);
      builder.Property(x => x.NeedToQualityControlDocumentUpload);
      builder.Property(x => x.IsTraceable);
      builder.Property(x => x.GrossWeight);
      builder.Property(x => x.Volume);
      builder.HasRowVersion();
      builder.Property(x => x.QualityControlDepartmentId);
      builder.Property(x => x.QualityControlEmployeeId);
      builder.Property(x => x.StuffHSGroupId);
      builder.Property(x => x.Tolerance);
      builder.Property(x => x.NetWeight);
      builder.Property(x => x.QualityControlCheckDuration);
      builder.Property(x => x.CeofficientSet);
      builder.Property(x => x.StuffDefinitionRequestId);
      builder.Property(x => x.ProjectHeaderId);
      builder.HasOne(x => x.StuffCategory).WithMany(x => x.Stuffs).HasForeignKey(x => x.StuffCategoryId);
      builder.HasOne(x => x.UnitType).WithMany(x => x.Stuffs).HasForeignKey(x => x.UnitTypeId);
      builder.HasOne(x => x.ProjectHeader).WithOne(x => x.Stuff).HasForeignKey<Stuff>(x => x.ProjectHeaderId);
      builder.HasOne(x => x.QualityControlDepartment).WithMany(x => x.Stuffs).HasForeignKey(x => x.QualityControlDepartmentId);
      builder.HasOne(x => x.QualityControlEmployee).WithMany(x => x.Stuffs).HasForeignKey(x => x.QualityControlEmployeeId);
      builder.HasOne(x => x.StuffHSGroup).WithMany(x => x.Stuffs).HasForeignKey(x => x.StuffHSGroupId);
      //builder.HasOne(x => x.StuffDefinitionRequest).WithOnePrincipal(x => x.Stuff);
    }
  }
}