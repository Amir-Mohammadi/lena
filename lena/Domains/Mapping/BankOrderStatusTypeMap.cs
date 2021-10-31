using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class BankOrderStatusTypeMap : IEntityTypeConfiguration<BankOrderStatusType>
  {
    public void Configure(EntityTypeBuilder<BankOrderStatusType> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("BankOrderStatusTypes");
      builder.Property(x => x.Id);
      builder.Property(x => x.Name).IsRequired();
      builder.HasRowVersion();
      builder.Property(x => x.Code).IsRequired();
    }
  }
}
