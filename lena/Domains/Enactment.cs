using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class Enactment : IEntity, IHasSaveLog
  {
    protected internal Enactment()
    {
      this.EnactmentActionProcessLogs = new HashSet<EnactmentActionProcessLog>();
    }
    public int Id { get; set; }
    public DateTime ActionDateTime { get; set; } // تاریخ اقدام
    public CollateralType CollateralType { get; set; } // نوع وثیقه
    public double CollateralAmount { get; set; }// مقدار وثیقه 
    public DateTime? ReceiveDateTime { get; set; }
    public int UserId { get; set; }
    public int BankOrderId { get; set; }
    public string Description { get; set; }
    public DateTime DateTime { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual User User { get; set; }
    public virtual BankOrder BankOrder { get; set; }
    public virtual ICollection<EnactmentActionProcessLog> EnactmentActionProcessLogs { get; set; }
    public virtual EnactmentActionProcessLog CurrentEnactmentActionProcessLog { get; set; }
  }
}