using System;

using lena.Domains.Enums;
namespace lena.Models.Accounting.BankOrderStateLog
{
  public class AddBankOrderStateLogInput
  {
    public int BankOrderId { get; set; }
    public int BankOrderStateId { get; set; }
    public DateTime DateTime { get; set; }
    public int UserId { get; set; }
    public string Description { get; set; }
  }
}
