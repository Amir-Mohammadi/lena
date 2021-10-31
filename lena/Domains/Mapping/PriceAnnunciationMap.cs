using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class PriceAnnunciationMap : IEntityTypeConfiguration<PriceAnnunciation>
  {
    public void Configure(EntityTypeBuilder<PriceAnnunciation> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("PriceAnnunciations");
      builder.Property(x => x.Id);
      builder.HasRowVersion();
      builder.Property(x => x.FromDate).HasColumnType("smalldatetime");
      builder.Property(x => x.ToDate).HasColumnType("smalldatetime");
      builder.Property(x => x.DocumentId);
      builder.Property(x => x.Status);
      builder.Property(x => x.RegisterarUserId);
      builder.Property(x => x.CooperatorId);
      builder.Property(x => x.RegisterDateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.Description);
      builder.HasOne(x => x.RegisterarUser).WithMany(x => x.PriceAnnunciations).HasForeignKey(x => x.RegisterarUserId);
      builder.HasOne(x => x.Cooperator).WithMany(x => x.PriceAnnunciations).HasForeignKey(x => x.CooperatorId);
    }
  }
}
