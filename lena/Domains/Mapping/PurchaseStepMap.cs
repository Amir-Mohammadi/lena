using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class PurchaseStepMap : IEntityTypeConfiguration<PurchaseStep>
  {
    public void Configure(EntityTypeBuilder<PurchaseStep> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_PurchaseStep");
      builder.Property(x => x.Id);
      builder.Property(x => x.HowToBuyDetailId);
      builder.Property(x => x.FollowUpDateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.IsCurrentStep);
      builder.Property(x => x.CargoItemId);
      builder.HasOne(x => x.HowToBuyDetail).WithMany(x => x.PurchaseSteps).HasForeignKey(x => x.HowToBuyDetailId);
      builder.HasOne(x => x.CargoItem).WithMany(x => x.PurchaseSteps).HasForeignKey(x => x.CargoItemId);
    }
  }
}
