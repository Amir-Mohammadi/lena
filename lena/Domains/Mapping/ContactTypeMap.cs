using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ContactTypeMap : IEntityTypeConfiguration<ContactType>
  {
    public void Configure(EntityTypeBuilder<ContactType> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("ContactTypes");
      builder.Property(x => x.Id);
      builder.Property(x => x.Type).IsRequired();
      builder.Property(x => x.Name).IsRequired();
      builder.HasRowVersion();
      builder.Property(x => x.EssentialContactType);
    }
  }
}
