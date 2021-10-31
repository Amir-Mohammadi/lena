using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class StuffDefinitionRequestMap : IEntityTypeConfiguration<StuffDefinitionRequest>
  {
    public void Configure(EntityTypeBuilder<StuffDefinitionRequest> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("StuffDefinitionRequests");
      builder.Property(x => x.Name).IsRequired();
      builder.Property(x => x.Noun).IsRequired();
      builder.Property(x => x.Title).IsRequired();
      builder.Property(x => x.Description);
      builder.Property(x => x.DefinitionStatus);
      builder.Property(x => x.StuffType);
      builder.HasRowVersion();
      builder.HasOne(x => x.User).WithMany(x => x.StuffDefinitionRequestRequesters).HasForeignKey(x => x.UserId);
      builder.HasOne(x => x.ConfirmerUser).WithMany(x => x.StuffDefinitionRequestConfirmers).HasForeignKey(x => x.ConfirmerUserId);
      builder.HasOne(x => x.Stuff).WithOne(x => x.StuffDefinitionRequest).HasForeignKey<Stuff>(x => x.StuffDefinitionRequestId);
      builder.HasOne(x => x.StuffPurchaseCategory).WithMany(x => x.StuffDefinitionRequests).HasForeignKey(x => x.StuffPurchaseCategoryId);
      builder.HasOne(x => x.UnitType).WithMany(x => x.StuffDefinitionRequests).HasForeignKey(x => x.UnitTypeId);
    }
  }
}