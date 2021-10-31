using lena.Domains.Enums;
using System;

using lena.Domains.Enums;
namespace lena.Models.Accounting.FinanceItemConfirmation
{
  public class FinanceItemConfirmationResult
  {
    public int Id { get; set; }
    public int UserId { get; set; }
    public string EmployeeName { get; set; }
    public DateTime DateTime { get; set; }
    public FinanceItemConfirmationStatus Status { get; set; }
  }
}
