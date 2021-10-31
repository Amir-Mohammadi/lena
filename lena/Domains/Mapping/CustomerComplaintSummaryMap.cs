using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class CustomerComplaintSummaryMap : IEntityTypeConfiguration<CustomerComplaintSummary>
  {
    public void Configure(EntityTypeBuilder<CustomerComplaintSummary> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("CustomerComplaintSummaries");
      builder.Property(x => x.Id);
      builder.HasRowVersion();
      builder.Property(x => x.ComplaintClassificationTypeDescription);
      builder.Property(x => x.ComplaintClassificationTypes);
      builder.Property(x => x.OccurrenceProbabilityStatus);
      builder.Property(x => x.OccurrenceSeverityStatus);
      builder.Property(x => x.DateOfAnnouncement);
      builder.Property(x => x.CustomerOpinion);
      builder.Property(x => x.CorrectiveAction);
      builder.Property(x => x.RiskLevelStatus);
      builder.Property(x => x.ComplaintTitle);
      builder.Property(x => x.DocumentId);
      builder.Property(x => x.QAOpinion);
      builder.Property(x => x.Status);
      builder.Property(x => x.CorrectiveActionUserId);
      builder.Property(x => x.CustomerComplaintId);
      builder.HasOne(x => x.CustomerComplaint).WithMany(x => x.CustomerComplaintSummaries).HasForeignKey(x => x.CustomerComplaintId);
      builder.HasOne(x => x.CorrectiveActionUser).WithMany(x => x.CustomerComplaintSummaries).HasForeignKey(x => x.CorrectiveActionUserId);
    }
  }
}
