using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class CustomerStuffMap : IEntityTypeConfiguration<CustomerStuff>
  {
    public void Configure(EntityTypeBuilder<CustomerStuff> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("CustomerStuffs");
      builder.Property(x => x.Id);
      builder.Property(x => x.Code).HasMaxLength(2).IsFixedLength();
      builder.Property(x => x.Name);
      builder.Property(x => x.StuffId);
      builder.Property(x => x.CustomerId);
      builder.Property(x => x.Type);
      builder.Property(x => x.ManufacturerCode).IsRequired().HasMaxLength(2).IsFixedLength();
      builder.Property(x => x.TechnicalNumber).IsRequired();
      builder.HasRowVersion();
      builder.HasOne(x => x.Customer).WithMany(x => x.CustomerStuffs).HasForeignKey(x => x.CustomerId);
      builder.HasOne(x => x.Stuff).WithMany(x => x.CustomerStuffs).HasForeignKey(x => x.StuffId);
    }
  }
}
