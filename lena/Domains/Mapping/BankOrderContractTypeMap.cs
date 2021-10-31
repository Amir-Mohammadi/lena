using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class BankOrderContractTypeMap : IEntityTypeConfiguration<BankOrderContractType>
  {
    public void Configure(EntityTypeBuilder<BankOrderContractType> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("BankOrderContractTypes");
      builder.Property(x => x.Id).ValueGeneratedOnAdd();
      builder.Property(x => x.Title).IsRequired();
      builder.Property(x => x.IsActive);
      builder.HasRowVersion();
    }
  }
}
