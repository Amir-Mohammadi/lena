using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class StockCheckingMap : IEntityTypeConfiguration<StockChecking>
  {
    public void Configure(EntityTypeBuilder<StockChecking> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("StockCheckings");
      builder.Property(x => x.Id);
      builder.Property(x => x.Title).IsRequired();
      builder.Property(x => x.StartDate).HasColumnType("smalldatetime");
      builder.Property(x => x.EndDate).HasColumnType("smalldatetime");
      builder.Property(x => x.Status);
      builder.Property(x => x.CreateDate).HasColumnType("smalldatetime");
      builder.Property(x => x.UserId);
      builder.Property(x => x.ActiveTagTypeId);
      builder.HasRowVersion();
      builder.Property(x => x.ShowInventory);
      builder.HasOne(x => x.User).WithMany(x => x.StockCheckings).HasForeignKey(x => x.UserId);
      builder.HasOne(x => x.ActiveTagType).WithMany(x => x.StockCheckings).HasForeignKey(x => x.ActiveTagTypeId);
    }
  }
}
