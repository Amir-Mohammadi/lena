using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class RiskStatusMap : IEntityTypeConfiguration<RiskStatus>
  {
    public void Configure(EntityTypeBuilder<RiskStatus> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("RiskStatuses");
      builder.Property(x => x.Id);
      builder.Property(x => x.RiskId).IsRequired();
      builder.Property(x => x.OccurrenceSeverityStatus).IsRequired();
      builder.Property(x => x.OccurrenceProbabilityStatus).IsRequired();
      builder.Property(x => x.UserId).IsRequired();
      builder.Property(x => x.DateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.RiskResolveId);
      builder.HasRowVersion();
      builder.HasOne(x => x.User).WithMany(x => x.RiskStatuses).HasForeignKey(x => x.UserId);
      builder.HasOne(x => x.Risk).WithMany(x => x.RiskStatuses).HasForeignKey(x => x.RiskId);//TODO fix it .WillCascadeOnDelete(true);
      builder.HasOne(x => x.RiskParameter).WithMany(x => x.RiskStatuses).HasForeignKey(x => new { x.OccurrenceSeverityStatus, x.OccurrenceProbabilityStatus });
      builder.HasOne(x => x.RiskResolve).WithOne(x => x.RiskStatus).HasForeignKey<RiskStatus>(x => x.RiskResolveId);
    }
  }
}