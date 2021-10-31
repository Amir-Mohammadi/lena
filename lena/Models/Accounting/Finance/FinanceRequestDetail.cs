using lena.Domains.Enums;
using System;

using lena.Domains.Enums;
namespace lena.Models.Accounting.Finance
{
  public class FinanceRequestDetail
  {
    public int Id { get; set; }
    public double? AllocatedAmount { get; set; }
    public DateTime? AcceptedDueDateTime { get; set; }
    public PaymentMethod? AcceptedPaymentMethod { get; set; }
    public string ChequeNumber { get; set; }
    public string FinancialDescription { get; set; }
    public FinanceItemConfirmationStatus DetermineStatus { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
