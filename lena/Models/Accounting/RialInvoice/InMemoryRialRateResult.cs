using System.Collections.Generic;

using lena.Domains.Enums;
namespace lena.Models.Accounting.RialInvoice
{
  public class InMemoryRialRateResult
  {
    public double? RialRateValue { get; set; }
    public List<InMemoryRialRate> InMemoryRialRates { get; set; }
  }
}
