using lena.Domains.Enums;
using System;

using lena.Domains.Enums;
namespace lena.Models.Accounting.FinanceConfirmation
{
  public class FinanceConfirmationResult
  {
    public int Id { get; set; }
    public int UserId { get; set; }
    public string EmployeeName { get; set; }
    public DateTime DateTime { get; set; }
    public FinanceConfirmationStatus Status { get; set; }
  }
}
