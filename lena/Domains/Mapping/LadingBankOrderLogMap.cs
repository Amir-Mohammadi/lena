using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class LadingBankOrderLogMap : IEntityTypeConfiguration<LadingBankOrderLog>
  {
    public void Configure(EntityTypeBuilder<LadingBankOrderLog> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("LadingBankOrderLogs");
      builder.Property(x => x.Id);
      builder.Property(x => x.LadingId);
      builder.HasRowVersion();
      builder.Property(x => x.DateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.UserId);
      builder.Property(x => x.Description);
      builder.Property(x => x.LadingBankOrderStatusId);
      builder.HasOne(x => x.User).WithMany(x => x.LadingBankOrderLogs).HasForeignKey(x => x.UserId);
      builder.HasOne(x => x.Lading).WithMany(x => x.LadingBankOrderLogs).HasForeignKey(x => x.LadingId);
      builder.HasOne(x => x.LadingBankOrderStatus).WithMany(x => x.LadingBankOrderLogs).HasForeignKey(x => x.LadingBankOrderStatusId);
    }
  }
}
