using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ProductionMap : IEntityTypeConfiguration<Production>
  {
    public void Configure(EntityTypeBuilder<Production> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("Productions");
      builder.HasSaveLog();
      builder.HasDescription();
      builder.HasRowVersion();
      builder.Property(x => x.Id);
      builder.Property(x => x.ProductionOrderId);
      builder.Property(x => x.StuffSerialCode);
      builder.Property(x => x.StuffSerialStuffId);
      builder.Property(x => x.Status);
      builder.Property(x => x.DateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.Type).IsRequired();
      builder.HasOne(x => x.ProductionOrder).WithMany(x => x.Productions).HasForeignKey(x => x.ProductionOrderId);
      builder.HasOne(x => x.StuffSerial).WithMany(x => x.Productions).HasForeignKey(x => new
      {
        x.StuffSerialCode,
        x.StuffSerialStuffId
      });
    }
  }
}
