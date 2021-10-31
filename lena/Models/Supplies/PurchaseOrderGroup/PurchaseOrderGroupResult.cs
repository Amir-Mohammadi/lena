using System;

using lena.Domains.Enums;
namespace lena.Models.Supplies.PurchaseOrderGroup
{
  public class PurchaseOrderGroupResult
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public DateTime DateTime { get; set; }
    public string Description { get; set; }
    public int FinancialTransacionBatchId { get; set; }
    public string EmployeeFullName { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
