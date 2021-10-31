using System;

using lena.Domains.Enums;
namespace lena.Models.Accounting.BillOfMaterialPriceHistory
{
  public class GetBillOfMaterialPriceHistoryDetailInput
  {
    public int? Id { get; set; }
    public int? BillOfMaterialPriceHistoryId { get; set; }
    public int? UserId { get; set; }
    public DateTime? DateTime { get; set; }
  }
}
