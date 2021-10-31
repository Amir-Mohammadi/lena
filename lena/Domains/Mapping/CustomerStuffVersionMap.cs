using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class CustomerStuffVersionMap : IEntityTypeConfiguration<CustomerStuffVersion>
  {
    public void Configure(EntityTypeBuilder<CustomerStuffVersion> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("CustomerStuffVersions");
      builder.Property(x => x.Id);
      builder.Property(x => x.Code).HasMaxLength(2).IsFixedLength();
      builder.Property(x => x.Name);
      builder.Property(x => x.IsPublish);
      builder.Property(x => x.CustomerStuffId);
      builder.HasRowVersion();
      builder.HasOne(x => x.CustomerStuff).WithMany(x => x.CustomerStuffVersions).HasForeignKey(x => x.CustomerStuffId);
    }
  }
}
