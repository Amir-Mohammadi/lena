using lena.Domains.Enums;
using System;

using lena.Domains.Enums;
namespace lena.Models.Accounting.FinanceAllocation
{
  public class FinanceAllocationDetailInput
  {
    public int Id { get; set; }
    public double Amount { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public DateTime AllocationDateTime { get; set; }
    public string ChequeNumber { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
