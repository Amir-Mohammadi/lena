using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ExitReceiptMap : IEntityTypeConfiguration<ExitReceipt>
  {
    public void Configure(EntityTypeBuilder<ExitReceipt> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_ExitReceipt");
      builder.Property(x => x.Id);
      builder.Property(x => x.Confirmed);
      builder.Property(x => x.OutboundCargoId);
      builder.Property(x => x.CooperatorId);
      builder.HasOne(x => x.OutboundCargo).WithMany(x => x.ExitReceipts).HasForeignKey(x => x.OutboundCargoId);
      builder.HasOne(x => x.Cooperator).WithMany(x => x.ExitReceipts).HasForeignKey(x => x.CooperatorId);
    }
  }
}
