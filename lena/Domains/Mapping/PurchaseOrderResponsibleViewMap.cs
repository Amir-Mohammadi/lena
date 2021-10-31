using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class PurchaseOrderResponsibleViewMap : IEntityTypeConfiguration<PurchaseOrderResponsibleView>
  {
    public void Configure(EntityTypeBuilder<PurchaseOrderResponsibleView> builder)
    {
      builder.ToTable("PurchaseOrderResponsibleView");
      builder.HasKey(x => x.PurchaseOrderId);
      builder.Ignore(x => x.RowVersion);
    }
  }
}