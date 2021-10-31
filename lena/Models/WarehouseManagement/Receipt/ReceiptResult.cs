using System;
using System.Linq;
using lena.Domains.Enums;
using lena.Models.WarehouseManagement.StoreReceipt;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.Receipt
{
  public class ReceiptResult
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public int CooperatorId { get; set; }
    public string CooperatorCode { get; set; }
    public string CooperatorName { get; set; }
    public string LadingCode { get; set; }
    public IQueryable<StoreReceiptResult> StoreReceipts { get; set; }
    public ReceiptStatus ReceiptStatus { get; set; }
    public StoreReceiptType StoreReceiptType { get; set; }
    public DateTime DateTime { get; set; }
    public DateTime? ReceiptDateTime { get; set; }
    public int UserId { get; set; }
    public string EmployeeFullName { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
