using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public class PurchaseRequestEditLog : IEntity
  {
    protected internal PurchaseRequestEditLog()
    { }
    public int Id { get; set; }
    public int PurchaseRequestId { get; set; }
    public Nullable<DateTime> AfterDeadLineDateTime { get; set; }
    public Nullable<DateTime> BeforeDeadLineDateTime { get; set; }
    public double? BeforeRequestQty { get; set; }
    public double? AfterRequestQty { get; set; }
    public string Description { get; set; }
    public int UserId { get; set; }
    public DateTime DateTime { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual User User { get; set; }
    public virtual PurchaseRequest PurchaseRequest { get; set; }
  }
}
