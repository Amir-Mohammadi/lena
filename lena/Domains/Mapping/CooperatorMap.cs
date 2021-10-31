using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class CooperatorMap : IEntityTypeConfiguration<Cooperator>
  {
    public void Configure(EntityTypeBuilder<Cooperator> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("Cooperators");
      builder.Property(x => x.Id);
      builder.Property(x => x.CooperatorType);
      builder.Property(x => x.ProviderType);
      builder.Property(x => x.Name).IsRequired();
      builder.Property(x => x.Code).HasMaxLength(25);
      builder.Property(x => x.DetailedCode).IsRequired();
      builder.Property(x => x.ConfirmationDetailedCode).IsRequired();
      builder.HasRowVersion();
      builder.HasOne(x => x.City).WithMany(x => x.Cooperators).HasForeignKey(x => x.CityId);
    }
  }
}