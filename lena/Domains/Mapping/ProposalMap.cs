using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ProposalMap : IEntityTypeConfiguration<Proposal>
  {
    public void Configure(EntityTypeBuilder<Proposal> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("Proposals");
      builder.Property(x => x.DateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.IsOpen);
      builder.Property(x => x.IsEffective);
      builder.Property(x => x.IsIncognitoUser);
      builder.Property(x => x.CurrentSituationDescription).IsRequired();
      builder.Property(x => x.ProposalDescription);
      builder.Property(x => x.ProposalEffect);
      builder.Property(x => x.Status).IsRequired();
      builder.Property(x => x.RowVersion).IsRowVersion().IsRequired();
      builder.HasOne(x => x.ProposalType).WithMany(x => x.Proposals).HasForeignKey(x => x.ProposalTypeId);
      builder.HasOne(x => x.User).WithMany(x => x.Proposals).HasForeignKey(x => x.UserId);
    }
  }
}