using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class LadingCustomhouseLogMap : IEntityTypeConfiguration<LadingCustomhouseLog>
  {
    public void Configure(EntityTypeBuilder<LadingCustomhouseLog> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("LadingCustomhouseLogs");
      builder.Property(x => x.Id);
      builder.Property(x => x.LadingId);
      builder.Property(x => x.LadingCustomhouseStatusId);
      builder.Property(x => x.Description);
      builder.Property(x => x.DateTime).HasColumnType("smalldatetime");
      builder.HasRowVersion();
      builder.Property(x => x.UserId);
      builder.HasOne(x => x.LadingCustomhouseStatus).WithMany(x => x.LadingCustomhouseLogs).HasForeignKey(x => x.LadingCustomhouseStatusId);
      builder.HasOne(x => x.Lading).WithMany(x => x.LadingCustomhouseLogs).HasForeignKey(x => x.LadingId);
      builder.HasOne(x => x.User).WithMany(x => x.LadingCustomhouseLogs).HasForeignKey(x => x.UserId);
    }
  }
}
