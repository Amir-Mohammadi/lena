using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class SerialProfileMap : IEntityTypeConfiguration<SerialProfile>
  {
    public void Configure(EntityTypeBuilder<SerialProfile> builder)
    {
      builder.HasKey(x => new
      {
        x.Code,
        x.StuffId
      });
      builder.ToTable("SerialProfiles");
      builder.Property(x => x.Code);
      builder.Property(x => x.StuffId);
      builder.Property(x => x.DateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.CooperatorId);
      builder.HasRowVersion();
      builder.Property(x => x.UserId);
      builder.HasOne(x => x.Stuff).WithMany(x => x.SerialProfiles).HasForeignKey(x => x.StuffId);
      builder.HasOne(x => x.Cooperator).WithMany(x => x.SerialProfiles).HasForeignKey(x => x.CooperatorId);
      builder.HasOne(x => x.User).WithMany(x => x.SerialProfiles).HasForeignKey(x => x.UserId);
    }
  }
}
