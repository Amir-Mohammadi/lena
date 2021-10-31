using System;

using lena.Domains.Enums;
namespace lena.Models.SaleManagement.OrderItemBlock
{
  public class SendProductSummaryResult
  {
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public int? CustomerId { get; set; }
    public string CustomerCode { get; set; }
    public string CustomerName { get; set; }
    public double SentQty { get; set; }
    public DateTime? SentDate { get; set; }
    public string ExitReceiptRequestTypeTitle { get; set; }
    public short CeofficientSet { get; set; }
    public short SumCeofficientSet { get; set; }
    public int? StuffCategoryId { get; set; }
    public string StuffCategoryName { get; set; }
    public bool? CeofficientSetType { get; set; }
  }
}
