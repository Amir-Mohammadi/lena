using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class QualityControlSummary : IEntity
  {
    protected internal QualityControlSummary()
    {
    }
    public int Id { get; set; }
    public double AcceptedQty { get; set; }
    public double FailedQty { get; set; }
    public double ConditionalRequestQty { get; set; }
    public double ConditionalQty { get; set; }
    public double ConditionalRejectedQty { get; set; }
    public double ReturnedQty { get; set; }
    public double ConsumedQty { get; set; }
    public int QualityControlId { get; internal set; }
    public byte[] RowVersion { get; set; }
    public virtual QualityControl QualityControl { get; set; }
  }
}