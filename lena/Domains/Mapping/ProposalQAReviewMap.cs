using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ProposalQAReviewMap : IEntityTypeConfiguration<ProposalQAReview>
  {
    public void Configure(EntityTypeBuilder<ProposalQAReview> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("ProposalQAReviews");
      builder.Property(x => x.ReviewDateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.DateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.ReviewResult).IsRequired();
      builder.HasRowVersion();
      builder.HasOne(x => x.Proposal).WithMany(x => x.QAReviews).HasForeignKey(x => x.ProposalId);
      builder.HasOne(x => x.User).WithMany(x => x.ProposalQAReviews).HasForeignKey(x => x.UserId);
      builder.HasOne(x => x.ResponsibleUser).WithMany(x => x.ProposalResponsibles).HasForeignKey(x => x.ResponsibleUserId);
    }
  }
}