using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ContactMap : IEntityTypeConfiguration<Contact>
  {
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("Contacts");
      builder.Property(x => x.Id);
      builder.Property(x => x.Title).IsRequired();
      builder.Property(x => x.ContactText).IsRequired();
      builder.Property(x => x.ContactTypeId);
      builder.Property(x => x.IsMain);
      builder.Property(x => x.CooperatorId);
      builder.Property(x => x.EmployeeId);
      builder.HasRowVersion();
      builder.HasOne(x => x.ContactType).WithMany(x => x.Contacts).HasForeignKey(x => x.ContactTypeId);
      builder.HasOne(x => x.Cooperator).WithMany(x => x.Contacts).HasForeignKey(x => x.CooperatorId);
      builder.HasOne(x => x.Employee).WithMany(x => x.Contacts).HasForeignKey(x => x.EmployeeId);
    }
  }
}
