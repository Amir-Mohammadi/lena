using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class AllocationMap : IEntityTypeConfiguration<Allocation>
  {
    public void Configure(EntityTypeBuilder<Allocation> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("Allocations");
      builder.Property(x => x.Id);
      builder.Property(x => x.BankOrderId).IsRequired();
      builder.Property(x => x.Amount);
      builder.Property(x => x.CurrencyId);
      builder.Property(x => x.Duration);
      builder.Property(x => x.Status);
      builder.Property(x => x.ReceivedDateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.BeginningDateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.FinalizationDateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.Description);
      builder.Property(x => x.UserId);
      builder.Property(x => x.DateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.StatisticalRegistrationCertificate);
      builder.Property(x => x.DocumentId);
      builder.HasRowVersion();
      builder.HasOne(x => x.BankOrder).WithMany(x => x.Allocations).HasForeignKey(x => x.BankOrderId);
      builder.HasOne(x => x.User).WithMany(x => x.Allocations).HasForeignKey(x => x.UserId);
    }
  }
}