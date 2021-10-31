using lena.Domains;

using lena.Domains.Enums;
namespace lena.Models.Accounting.RialInvoice
{
  /// <summary>
  /// نرخ ریالی که با آن بستانکار را صاف کردیم
  /// </summary>
  public class SettledRialRate
  {
    /// <summary>
    /// برای اطمینان از ترتیب درست در لیست
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// نرخ ریالی که با آن بستانکار را صاف کرده ایم
    /// </summary>
    public RialRate RialRate { get; set; }

    public InMemoryRialRate InMemoryRialRate { get; set; }

    /// <summary>
    /// مبلغی از نرخ ریالی که برای صاف کردن و ریالی کردن بستانکار مربوطه استفاده شده است
    /// </summary>
    public double UsedAmount { get; set; }
  }
}
