using System;

using lena.Domains.Enums;
namespace lena.Models.Accounting.RialInvoice
{
  public class InMemoryRialRate
  {
    public Guid Id { get; set; }
    public double Amount { get; set; }
    public double Rate { get; set; }
    public int FinancialTransactionId { get; set; }
    public Guid? ReferenceRialRateId { get; set; }
  }
}
