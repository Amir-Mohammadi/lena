using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class IranKhodroSerialMap : IEntityTypeConfiguration<IranKhodroSerial>
  {
    public void Configure(EntityTypeBuilder<IranKhodroSerial> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("IranKhodroSerials");
      builder.Property(x => x.Id);
      builder.Property(x => x.ProductionYearId);
      builder.Property(x => x.CustomerStuffId);
      builder.Property(x => x.CustomerStuffVersionId);
      builder.Property(x => x.ProductionSerial).IsRequired();//TODO fix it.HasColumnAnnotation("Range", new RangeAttribute(0000, 9999));
      builder.Property(x => x.ProductionDay).IsRequired();//TODO fix it.HasColumnAnnotation("Range", new RangeAttribute(000, 999));      
      builder.Property(x => x.ProductionDateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.UserId);
      builder.Property(x => x.LinkSerialId);
      builder.Property(x => x.Print);
      builder.Property(x => x.PrintQty);
      builder.HasRowVersion();
      builder.HasOne(x => x.User).WithMany(x => x.IranKhodroSerials).HasForeignKey(x => x.UserId);
      builder.HasOne(x => x.CustomerStuff).WithMany(x => x.IranKhordoSerials).HasForeignKey(x => x.CustomerStuffId);
      builder.HasOne(x => x.CustomerStuffVersion).WithMany(x => x.IranKhordoSerials).HasForeignKey(x => x.CustomerStuffVersionId);
      builder.HasOne(x => x.ProductionYear).WithMany(x => x.IranKhordoSerials).HasForeignKey(x => x.ProductionYearId);
      builder.HasOne(x => x.LinkSerial).WithOne(x => x.IranKhodroSerial).HasForeignKey<IranKhodroSerial>(x => x.LinkSerialId);
    }
  }
}