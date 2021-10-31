using lena.Domains.Enums;
namespace lena.Models.Accounting.FinancialDocument
{
  public class PurchaseOrderDiscountResult
  {
    public int Id { get; set; }
    public double Amount { get; set; }
    public int? PurchaseOrderGroupId { get; set; }
    public int PurchaseOrderId { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
