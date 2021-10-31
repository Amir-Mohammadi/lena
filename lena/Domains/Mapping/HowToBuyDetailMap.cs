using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class HowToBuyDetailMap : IEntityTypeConfiguration<HowToBuyDetail>
  {
    public void Configure(EntityTypeBuilder<HowToBuyDetail> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("HowToBuyDetails");
      builder.Property(x => x.Id);
      builder.Property(x => x.Title).IsRequired();
      builder.Property(x => x.Order);
      builder.Property(x => x.HowToBuyId);
      builder.HasRowVersion();
      builder.HasOne(x => x.HowToBuy).WithMany(x => x.HowToBuyDetails).HasForeignKey(x => x.HowToBuyId);
    }
  }
}
