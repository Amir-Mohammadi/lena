using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class PrinterMap : IEntityTypeConfiguration<Printer>
  {
    public void Configure(EntityTypeBuilder<Printer> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("Printers");
      builder.Property(x => x.Id);
      builder.Property(x => x.NameInSystem).IsRequired();
      builder.Property(x => x.NetworkAddress);
      builder.Property(x => x.Manufacture).IsRequired();
      builder.Property(x => x.Model);
      builder.Property(x => x.Logo).IsRequired();
      builder.Property(x => x.IsColored);
      builder.Property(x => x.PrinterType);
      builder.Property(x => x.Location);
      builder.Property(x => x.SupportLan);
      builder.Property(x => x.ModuleName);
      builder.Property(x => x.IsActive);
      builder.Property(x => x.IsSupportTemplatePrint);
      builder.Property(x => x.Setting);
      builder.Property(x => x.CreationTime).HasColumnType("smalldatetime");
      builder.Property(x => x.Description);
      builder.HasRowVersion();
      builder.Property(x => x.CreatorUserId);
      builder.HasOne(x => x.CreatorUser).WithMany(x => x.Printers).HasForeignKey(x => x.CreatorUserId);
    }
  }
}
