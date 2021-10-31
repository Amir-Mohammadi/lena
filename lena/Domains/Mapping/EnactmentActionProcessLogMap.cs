using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class EnactmentActionProcessLogMap : IEntityTypeConfiguration<EnactmentActionProcessLog>
  {
    public void Configure(EntityTypeBuilder<EnactmentActionProcessLog> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("EnactmentActionProcessLogs");
      builder.Property(x => x.Id);
      builder.Property(x => x.EnactmentId);
      builder.Property(x => x.EnactmentActionProcessId);
      builder.Property(x => x.Description);
      builder.Property(x => x.DateTime).HasColumnType("smalldatetime");
      builder.HasRowVersion();
      builder.Property(x => x.UserId);
      builder.HasOne(x => x.EnactmentActionProcess).WithMany(x => x.EnactmentActionProcessLogs).HasForeignKey(x => x.EnactmentActionProcessId);
      builder.HasOne(x => x.Enactment).WithMany(x => x.EnactmentActionProcessLogs).HasForeignKey(x => x.EnactmentId);
      builder.HasOne(x => x.User).WithMany(x => x.EnactmentActionProcessLogs).HasForeignKey(x => x.UserId);
    }
  }
}
