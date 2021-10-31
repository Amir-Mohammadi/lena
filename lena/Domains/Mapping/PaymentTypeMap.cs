using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class PaymentTypeMap : IEntityTypeConfiguration<PaymentType>
  {
    public void Configure(EntityTypeBuilder<PaymentType> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("PaymentTypes");
      builder.Property(x => x.Id);
      builder.Property(x => x.Name);
      builder.HasRowVersion();
      builder.Property(x => x.IsActive);
    }
  }
}
