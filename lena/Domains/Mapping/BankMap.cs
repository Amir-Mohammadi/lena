using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class BankMap : IEntityTypeConfiguration<Bank>
  {
    public void Configure(EntityTypeBuilder<Bank> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("Banks");
      builder.Property(x => x.Id).ValueGeneratedOnAdd();
      builder.Property(x => x.Title).IsRequired();
      builder.Property(x => x.IsActive);
      builder.Property(x => x.Description);
      builder.HasRowVersion();
    }
  }
}
