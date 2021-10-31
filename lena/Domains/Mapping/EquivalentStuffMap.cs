using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class EquivalentStuffMap : IEntityTypeConfiguration<EquivalentStuff>
  {
    public void Configure(EntityTypeBuilder<EquivalentStuff> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("EquivalentStuffs");
      builder.Property(x => x.Id);
      builder.Property(x => x.Title).IsRequired();
      builder.Property(x => x.IsActive);
      builder.Property(x => x.BillOfMaterialDetailId);
      builder.Property(x => x.Description);
      builder.HasRowVersion();
      builder.Property(x => x.EquivalentStuffType);
      builder.HasOne(x => x.BillOfMaterialDetail).WithMany(x => x.EquivalentStuffs).HasForeignKey(x => x.BillOfMaterialDetailId);
    }
  }
}
