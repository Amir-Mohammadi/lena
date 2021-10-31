using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ProposalReviewCommitteeMap : IEntityTypeConfiguration<ProposalReviewCommittee>
  {
    public void Configure(EntityTypeBuilder<ProposalReviewCommittee> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("ProposalReviewCommittees");
      builder.Property(x => x.DateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.ReviewDateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.ReviewResult).IsRequired();
      builder.HasRowVersion();
      builder.HasOne(x => x.ResponsibleUser).WithMany(x => x.ProposalReviewCommitteeResponsibles).HasForeignKey(x => x.ResponsibleUserId);
      builder.HasOne(x => x.User).WithMany(x => x.ProposalReviewCommittees).HasForeignKey(x => x.UserId);
      builder.HasOne(x => x.Proposal).WithMany(x => x.ReviewCommittees).HasForeignKey(x => x.ProposalId);
    }
  }
}
