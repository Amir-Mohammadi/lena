using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class StuffProviderMap : IEntityTypeConfiguration<StuffProvider>
  {
    public void Configure(EntityTypeBuilder<StuffProvider> builder)
    {
      builder.HasKey(x => new
      {
        x.StuffId,
        x.ProviderId
      });
      builder.ToTable("StuffProviders");
      builder.Property(x => x.StuffId);
      builder.Property(x => x.ProviderId);
      builder.Property(x => x.LeadTime);
      builder.Property(x => x.IsActive);
      builder.Property(x => x.IsDefault);
      builder.Property(x => x.Description);
      builder.Property(x => x.InstantLeadTime);
      builder.HasRowVersion();
      builder.HasOne(x => x.Stuff).WithMany(x => x.StuffProviders).HasForeignKey(x => x.StuffId);
      builder.HasOne(x => x.Provider).WithMany(x => x.StuffProviders).HasForeignKey(x => x.ProviderId);
    }
  }
}