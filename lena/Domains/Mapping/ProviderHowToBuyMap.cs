using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ProviderHowToBuyMap : IEntityTypeConfiguration<ProviderHowToBuy>
  {
    public void Configure(EntityTypeBuilder<ProviderHowToBuy> builder)
    {
      builder.HasKey(x => new { x.ProviderId, x.HowToBuyId });
      builder.ToTable("ProviderHowToBuys");
      builder.Property(x => x.ProviderId);
      builder.Property(x => x.HowToBuyId);
      builder.Property(x => x.LeadTime);
      builder.Property(x => x.IsActive);
      builder.Property(x => x.IsDefault);
      builder.Property(x => x.Description);
      builder.HasRowVersion();
      builder.HasOne(x => x.Provider).WithMany(x => x.ProviderHowToBuys).HasForeignKey(x => x.ProviderId);
      builder.HasOne(x => x.HowToBuy).WithMany(x => x.ProviderHowToBuys).HasForeignKey(x => x.HowToBuyId);
    }
  }
}