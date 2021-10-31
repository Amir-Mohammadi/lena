using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class LinkSerialMap : IEntityTypeConfiguration<LinkSerial>
  {
    public void Configure(EntityTypeBuilder<LinkSerial> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("LinkSerials");
      builder.Property(x => x.LinkedSerial).IsRequired().HasMaxLength(50);
      builder.HasIndex(x => x.LinkedSerial).IsUnique();
      builder.Property(x => x.UserId);
      builder.Property(x => x.UserLinkerId);
      builder.Property(x => x.DateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.LinkDateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.CustomerId);
      builder.Property(x => x.Type);
      builder.Property(x => x.StuffSerialCode);
      builder.Property(x => x.StuffId);
      builder.HasRowVersion();
      builder.HasOne(x => x.User).WithMany(x => x.LinkSerials).HasForeignKey(x => x.UserId);
      builder.HasOne(x => x.UserLinker).WithMany(x => x.LinkerLinkSerials).HasForeignKey(x => x.UserLinkerId);
      builder.HasOne(x => x.Customer).WithMany(x => x.LinkSerials).HasForeignKey(x => x.CustomerId);
      builder.HasOne(x => x.StuffSerial).WithOne(x => x.LinkSerial).HasForeignKey<LinkSerial>(x => new { x.StuffSerialCode, x.StuffId });
    }
  }
}