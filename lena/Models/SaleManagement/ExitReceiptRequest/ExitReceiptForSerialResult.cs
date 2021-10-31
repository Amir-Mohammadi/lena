using lena.Domains.Enums;
using System;

using lena.Domains.Enums;
namespace lena.Models.SaleManagement.ExitReceiptRequest
{
  public class ExitReceiptForSerialResult
  {
    public int? ExitReceiptId { get; set; }
    public int? SendProductId { get; set; }
    public string SendProductCode { get; set; }
    public DateTime? SendProductDateTime { get; set; }
    public int? StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public StuffType StuffType { get; set; }

    public double? Qty { get; set; }
    public int? UnitId { get; set; }
    public string UnitName { get; set; }
    public string Serial { get; set; }
    public int? CooperatorId { get; set; }
    public string CooperatorCode { get; set; }
    public string CooperatorName { get; set; }
    public int? BillOfMaterialVersion { get; set; }
    public byte[] RowVersion { get; set; }
    public int PreparingSendingStuffId { get; set; }
  }
}
