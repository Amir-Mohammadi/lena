using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class SecurityActionMap : IEntityTypeConfiguration<SecurityAction>
  {
    public void Configure(EntityTypeBuilder<SecurityAction> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("SecurityActions");
      builder.Property(x => x.Id);
      builder.Property(x => x.Name).IsRequired().HasMaxLength(512);
      builder.Property(x => x.ActionName).IsRequired();
      builder.HasRowVersion();
      builder.Property(x => x.SecurityActionGroupId);
      builder.HasOne(x => x.SecurityActionGroup).WithMany(x => x.SecurityActions).HasForeignKey(x => x.SecurityActionGroupId);
    }
  }
}
