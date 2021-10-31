using System;

using lena.Domains.Enums;
namespace lena.Models.Supplies.PurchaseOrder
{
  public class ValidationSaveCargoResult

  {
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public DateTime Deadline { get; set; }
    public bool IsValid { get; set; }

  }
}
