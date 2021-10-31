using System;
using System.Linq;
using lena.Domains.Enums;
using lena.Models.Supplies.EnactmentActionProcessLog;

using lena.Domains.Enums;
namespace lena.Models
{
  public class EnactmentResult
  {
    public int Id { get; set; }
    public int BankOrderId { get; set; }
    public string BankOrderNumber { get; set; }
    public DateTime ActionDateTime { get; set; } // تاریخ اقدام
    public CollateralType CollateralType { get; set; } // نوع وثیقه
    public double CollateralAmount { get; set; }// مقدار وثیقه 
    public DateTime? ReceiveDateTime { get; set; }
    public int UserId { get; set; }
    public string EmployeeFullName { get; set; }
    public string Description { get; set; }
    public DateTime DateTime { get; set; }
    public IQueryable<EnactmentActionProcessLogResult> EnactmentActionProcessLogs { get; set; }
    public byte[] RowVersion { get; set; }

  }
}