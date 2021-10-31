using System;

using lena.Domains.Enums;
namespace lena.Models
{
  public class AddAllocationInput
  {
    public string FileKey { get; set; }
    public int BankOrderId { get; set; }
    public string BankOrderNumber { get; set; }
    public double Amount { get; set; }
    public byte CurrencyId { get; set; }
    public int Duration { get; set; } // مدت فیش
                                      //public AllocationStatus Status { get; set; } //مرحله اقدام
    public DateTime ReceivedDateTime { get; set; } // تاریخ دریافت
    public DateTime BeginningDateTime { get; set; } // تاریخ اقدام
                                                    //public DateTime FinalizationDateTime { get; set; } // تاریخ اتمام
    public int StatisticalRegistrationCertificate { get; set; }//شماره گواهی ثبت آماری                  
    public string Description { get; set; }

  }
}