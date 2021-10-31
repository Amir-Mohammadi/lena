using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class QualityControlItem : BaseEntity, IEntity
  {
    protected internal QualityControlItem()
    {
      this.QualityControlSamples = new HashSet<QualityControlSample>();
    }
    public int QualityControlId { get; set; }
    public int StuffId { get; set; }
    public long StuffSerialCode { get; set; }
    public double Qty { get; set; }
    public byte UnitId { get; set; }
    public bool Status { get; set; }
    public Nullable<int> ReturnOfSaleId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual QualityControl QualityControl { get; set; }
    public virtual Stuff Stuff { get; set; }
    public virtual StuffSerial StuffSerial { get; set; }
    public virtual QualityControlConfirmationItem QualityControlConfirmationItem { get; set; }
    public virtual Unit Unit { get; set; }
    public virtual ReturnOfSale ReturnOfSale { get; set; }
    public virtual ICollection<QualityControlSample> QualityControlSamples { get; set; }
  }
}