using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class InboundCargoCooperatorMap : IEntityTypeConfiguration<InboundCargoCooperator>
  {
    public void Configure(EntityTypeBuilder<InboundCargoCooperator> builder)
    {
      builder.HasKey(x => new
      {
        x.InboundCargoId,
        x.CooperatorId
      });
      builder.ToTable("InboundCargoCooperators");
      builder.Property(x => x.CooperatorId);
      builder.Property(x => x.InboundCargoId);
      builder.HasRowVersion();
      builder.HasOne(x => x.Cooperator).WithMany(x => x.InboundCargoCooperators).HasForeignKey(x => x.CooperatorId);
      builder.HasOne(x => x.InboundCargo).WithMany(x => x.InboundCargoCooperators).HasForeignKey(x => x.InboundCargoId);
    }
  }
}
