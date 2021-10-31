using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class BankOrderLogMap : IEntityTypeConfiguration<BankOrderLog>
  {
    public void Configure(EntityTypeBuilder<BankOrderLog> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("BankOrderLogs");
      builder.Property(x => x.Id);
      builder.Property(x => x.BankOrderStatusTypeId);
      builder.Property(x => x.BankOrderId);
      builder.Property(x => x.UserId);
      builder.Property(x => x.DateTime).HasColumnType("smalldatetime");
      builder.HasRowVersion();
      builder.Property(x => x.Description);
      builder.HasOne(x => x.BankOrderStatusType).WithMany(x => x.BankOrderLogs).HasForeignKey(x => x.BankOrderStatusTypeId);
      builder.HasOne(x => x.BankOrder).WithMany(x => x.BankOrderLogs).HasForeignKey(x => x.BankOrderId);
    }
  }
}
