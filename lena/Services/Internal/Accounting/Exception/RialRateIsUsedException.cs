using System;
using System.Globalization;
using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting.Exception
{
  public class RialRateIsUsedException : InternalServiceException
  {
    public int FinancialTransactionId { get; set; }
    public string FinancialAccountCode { get; set; }
    public string EffectPersianDate { get; set; }

    public RialRateIsUsedException(int financialTransactionId, string financialAccountCode, DateTime effectDateTime)
    {
      FinancialTransactionId = financialTransactionId;
      FinancialAccountCode = financialAccountCode;

      PersianCalendar pc = new PersianCalendar();
      effectDateTime = effectDateTime.ToLocalTime();
      var year = pc.GetYear(effectDateTime).ToString("00");
      var month = pc.GetMonth(effectDateTime).ToString("00");
      var day = pc.GetDayOfMonth(effectDateTime).ToString("00");

      var hour = pc.GetHour(effectDateTime).ToString("00");
      var minute = pc.GetMinute(effectDateTime).ToString("00");

      EffectPersianDate = $"{hour}:{minute} {year}/{month}/{day}";
    }
  }
}
