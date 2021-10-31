using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ReturnOfSaleStatusLogMap : IEntityTypeConfiguration<ReturnOfSaleStatusLog>
  {
    public void Configure(EntityTypeBuilder<ReturnOfSaleStatusLog> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntityLogs_ReturnOfSaleStatusLog");
      builder.Property(x => x.Id);
      builder.Property(x => x.Status);
      builder.Property(x => x.ReturnOfSaleId);
      builder.HasOne(x => x.ReturnOfSale).WithMany(x => x.ReturnOfSaleStatusLogs).HasForeignKey(x => x.ReturnOfSaleId);
    }
  }
}
