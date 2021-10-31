using System;
using lena.Domains.Enums;
using lena.Models.Supplies.EnactmentActionProcessLog;

using lena.Domains.Enums;
namespace lena.Models
{
  public class AddEnactmentInput
  {
    public int BankOrderId { get; set; }
    public string Description { get; set; }
    public double CollateralAmount { get; set; } // مقدار وثیقه
    public CollateralType CollateralType { get; set; } // نوع وثیقه                                                           
    public AddEnactmentActionProcessLogInput[] EnactmentActionProcessLogs { get; set; }
    public DateTime? ReceiveDateTime { get; set; } //تاریخ دریافت
    public DateTime ActionDateTime { get; set; } // تاریخ اقدام

  }
}