using lena.Domains.Enums;
namespace lena.Models.Accounting.RialInvoice
{
  /// <summary>
  /// تراکنش های بستانکاری و میزان مبلغ آن که توسط یک تراکنش انتقال-واریز صاف میشود
  /// </summary>
  public class BestanTransactionRelatedToTransferDepositRialRate
  {
    /// <summary>
    /// برای اطمینان از ترتیب درست تراکنش ها
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// تراکنش بستانکاری که توسط تراکنش انتقال-واریز مربوطه صاف میشود
    /// </summary>
    public Domains.FinancialTransaction BestanFinancialTransaction { get; set; }
    /// <summary>
    /// مبلغی از تراکنش بستانکاری که توسط تراکنش انتقال-واریز مربوطه صاف میشود
    /// </summary>
    public double RelatedAmount { get; set; }
    /// <summary>
    /// بخشی از RelatedAmount که ریالی شده است
    /// </summary>
    public double SettledAmount { get; set; }
  }
}