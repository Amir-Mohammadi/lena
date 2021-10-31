using lena.Domains.Enums;
using System;

using lena.Domains.Enums;
namespace lena.Models.Accounting.FinanceAllocation
{
  public class FinanceAllocationResult
  {
    public int Id { get; set; }
    public int FinanceId { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public double Amount { get; set; }
    public string ChequeNumber { get; set; }
    public string EmployeeName { get; set; }
    public int UserId { get; set; }
    public DateTime DateTime { get; set; }
    public DateTime AllocationDateTime { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
