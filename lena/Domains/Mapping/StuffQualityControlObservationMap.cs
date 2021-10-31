using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class StuffQualityControlObservationMap : IEntityTypeConfiguration<StuffQualityControlObservation>
  {
    public void Configure(EntityTypeBuilder<StuffQualityControlObservation> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("StuffQualityControlObservations");
      builder.Property(x => x.Id);
      builder.HasRowVersion();
      builder.Property(x => x.Description);
      builder.Property(x => x.RegisterDateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.RegisterarUserId);
      builder.Property(x => x.StuffId);
      builder.HasOne(x => x.RegisterarUser).WithMany(x => x.StuffQualityControlObservations).HasForeignKey(x => x.RegisterarUserId);
      builder.HasOne(x => x.Stuff).WithMany(x => x.StuffQualityControlObservations).HasForeignKey(x => x.StuffId);
    }
  }
}