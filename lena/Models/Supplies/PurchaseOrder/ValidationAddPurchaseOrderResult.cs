using System;

using lena.Domains.Enums;
namespace lena.Models.Supplies.PurchaseOrder
{
  public class ValidationAddPurchaseOrderResult
  {

    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public DateTime Deadline { get; set; }
    public bool IsValid { get; set; }
  }
}