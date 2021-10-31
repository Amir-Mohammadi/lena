using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class QualityControlSample : IEntity
  {
    protected internal QualityControlSample()
    {
    }
    public int Id { get; set; }
    public string Code { get; set; }
    public int UserId { get; set; }
    public int? StatusChangerUserId { get; set; }
    public DateTime DateTime { get; set; }
    public double Qty { get; set; }
    public double? TestQty { get; set; }
    public double? ConsumeQty { get; set; }
    public QualityControlSampleStatus Status { get; set; }
    public byte[] RowVersion { get; set; }
    public int QualityControlItemId { get; set; }
    public virtual User User { get; set; }
    public virtual User StatusChangerUser { get; set; }
    public virtual QualityControlItem QualityControlItem { get; set; }
  }
}