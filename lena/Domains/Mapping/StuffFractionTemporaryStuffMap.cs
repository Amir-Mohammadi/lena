using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class StuffFractionTemporaryStuffMap : IEntityTypeConfiguration<StuffFractionTemporaryStuff>
  {
    public void Configure(EntityTypeBuilder<StuffFractionTemporaryStuff> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("StuffFractionTemporaryStuffs");
      builder.Property(x => x.DateTime).HasColumnType("smalldatetime").IsRequired();
      builder.Property(x => x.ProjectCode).IsRequired(false);
      builder.Property(x => x.Qty).IsRequired();
      builder.HasRowVersion();
      builder.HasOne(x => x.User).WithMany(x => x.StuffFractionReportTemporaryStuffs).HasForeignKey(x => x.UserId);
      builder.HasOne(x => x.Stuff).WithMany(x => x.StuffFractionTemporaryStuffs).HasForeignKey(x => x.StuffId);
    }
  }
}