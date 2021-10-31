using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ProposalTypeMap : IEntityTypeConfiguration<ProposalType>
  {
    public void Configure(EntityTypeBuilder<ProposalType> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("ProposalTypes");
      builder.Property(x => x.Title).IsRequired().IsUnicode();
      builder.Property(x => x.Description).IsRequired(false).IsUnicode();
      builder.Property(x => x.IsActive).IsRequired();
      builder.Property(x => x.RowVersion).IsRowVersion().IsRequired();
    }
  }
}