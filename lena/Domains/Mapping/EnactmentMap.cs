using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class EnactmentMap : IEntityTypeConfiguration<Enactment>
  {
    public void Configure(EntityTypeBuilder<Enactment> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("Enactments");
      builder.Property(x => x.Id);
      builder.Property(x => x.ActionDateTime).HasColumnType("smalldatetime").IsRequired();
      builder.Property(x => x.CollateralType).IsRequired();
      builder.Property(x => x.CollateralAmount).IsRequired();
      builder.Property(x => x.ReceiveDateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.UserId).IsRequired();
      builder.Property(x => x.BankOrderId).IsRequired();
      builder.Property(x => x.Description);
      builder.Property(x => x.DateTime).HasColumnType("smalldatetime").IsRequired();
      builder.HasRowVersion();
    }
  }
}