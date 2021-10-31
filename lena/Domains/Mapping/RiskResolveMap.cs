using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class RiskResolveMap : IEntityTypeConfiguration<RiskResolve>
  {
    public void Configure(EntityTypeBuilder<RiskResolve> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("RiskResolves");
      builder.Property(x => x.Id);
      builder.Property(x => x.CorrectiveAction).IsRequired();
      builder.Property(x => x.CreatorUserId).IsRequired();
      builder.Property(x => x.DateTime).HasColumnType("smalldatetime").IsRequired();
      builder.Property(x => x.Status);
      builder.Property(x => x.ReviewerUserId).IsRequired(false);
      builder.Property(x => x.RevieweDateTime).HasColumnType("smalldatetime").IsRequired(false);
      builder.Property(x => x.ReviewDescription).IsRequired(false);
      builder.HasRowVersion();
      builder.HasOne(x => x.CreatorUser).WithMany(x => x.RiskResolveCreator).HasForeignKey(x => x.CreatorUserId);
      builder.HasOne(x => x.ReviewerUser).WithMany(x => x.RiskResolveReviewer).HasForeignKey(x => x.ReviewerUserId);
      builder.HasOne(x => x.Risk).WithMany(x => x.RiskResolves).HasForeignKey(x => x.RiskId);
    }
  }
}