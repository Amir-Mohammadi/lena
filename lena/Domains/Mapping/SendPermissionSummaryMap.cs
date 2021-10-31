using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class SendPermissionSummaryMap : IEntityTypeConfiguration<SendPermissionSummary>
  {
    public void Configure(EntityTypeBuilder<SendPermissionSummary> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("SendPermissionSummaries");
      builder.Property(x => x.Id);
      builder.Property(x => x.PreparingSendingQty);
      builder.Property(x => x.SendedQty);
      builder.Property(x => x.SendPermissionId);
      builder.HasRowVersion();
      builder.HasOne(x => x.SendPermission).WithOne(x => x.SendPermissionSummary).HasForeignKey<SendPermissionSummary>(x => x.SendPermissionId);
    }
  }
}