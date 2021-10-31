using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class BankOrderIssueTypeMap : IEntityTypeConfiguration<BankOrderIssueType>
  {
    public void Configure(EntityTypeBuilder<BankOrderIssueType> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("BankOrderIssueTypes");
      builder.Property(x => x.Id);
      builder.Property(x => x.Name);
      builder.HasRowVersion();
    }
  }
}
