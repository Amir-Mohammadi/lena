using lena.Domains.Enums;
using lena.Models.Supplies.EnactmentActionProcessLog;
using System;

using lena.Domains.Enums;
namespace lena.Models
{
  public class EditEnactmentInput
  {
    public int Id { get; set; }
    public string Description { get; set; }
    public double CollateralAmount { get; set; } // مقدار وثیقه
    public CollateralType CollateralType { get; set; } // نوع وثیقه 
    public DateTime? ReceiveDateTime { get; set; } //تاریخ دریافت
    public DateTime ActionDateTime { get; set; } // تاریخ اقدام                                                   
    public int[] DeleteEnactmentActionProcessIds { get; set; }
    public AddEnactmentActionProcessLogInput[] EnactmentActionProcessLogs { get; set; } // مرحله اقدام
    public byte[] RowVersion { get; set; }

  }
}