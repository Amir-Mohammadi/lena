using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class StuffPurchaseCategoryMap : IEntityTypeConfiguration<StuffPurchaseCategory>
  {
    public void Configure(EntityTypeBuilder<StuffPurchaseCategory> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("StuffPurchaseCategories");
      builder.Property(x => x.Title);
      builder.Property(x => x.Description);
      builder.Property(x => x.Code);
      builder.Property(x => x.QualityControlUserGroupId);
      builder.Property(x => x.QualityControlDepartmentId);
      builder.HasRowVersion();
      builder.HasOne(x => x.StuffDefinitionUserGroup)
          .WithMany()
          .HasForeignKey(x => x.StuffDefinitionUserGroupId)
          ;
      builder.HasOne(x => x.StuffDefinitionConfirmerUserGroup)
           .WithMany()
           .HasForeignKey(x => x.StuffDefinitionConfirmerUserGroupId)
           ;
      builder.HasMany(x => x.Details).WithOne(x => x.StuffPurchaseCategory).HasForeignKey(x => x.StuffPurchaseCategoryId);
      builder.HasMany(x => x.Stuffs).WithOne(x => x.StuffPurchaseCategory).HasForeignKey(x => x.StuffPurchaseCategoryId);
      builder.HasOne(x => x.QualityControlUserGroup).WithMany(x => x.StuffPurchaseCategories).HasForeignKey(x => x.QualityControlUserGroupId);
      builder.HasOne(x => x.QualityControlDepartment).WithMany(x => x.StuffPurchaseCategories).HasForeignKey(x => x.QualityControlDepartmentId);
    }
  }
}