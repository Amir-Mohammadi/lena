using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class StuffPurchaseCategoryDetailMap : IEntityTypeConfiguration<StuffPurchaseCategoryDetail>
  {
    public void Configure(EntityTypeBuilder<StuffPurchaseCategoryDetail> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("StuffPurchaseCategoryDetails");
      builder.HasRowVersion();
      builder.HasOne(x => x.StuffPurchaseCategory).WithMany(x => x.Details).HasForeignKey(x => x.StuffPurchaseCategoryId);
      builder.HasOne(x => x.ApplicatorUserGroup).WithMany().HasForeignKey(x => x.ApplicatorUserGroupId);
      builder.HasOne(x => x.ApplicatorConfirmerUserGroup).WithMany().HasForeignKey(x => x.ApplicatorConfirmerUserGroupId);
      builder.HasOne(x => x.RequestConfirmerUserGroup).WithMany().HasForeignKey(x => x.RequestConfirmerUserGroupId);
    }
  }
}