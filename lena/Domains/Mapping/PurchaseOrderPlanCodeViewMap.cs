using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class PurchaseOrderPlanCodeViewMap : IEntityTypeConfiguration<PurchaseOrderPlanCodeView>
  {
    public void Configure(EntityTypeBuilder<PurchaseOrderPlanCodeView> builder)
    {
      builder.ToTable("PurchaseOrderPlanCodeView");
      builder.HasKey(x => x.PurchaseOrderId);
      builder.Ignore(x => x.RowVersion);
    }
  }
}