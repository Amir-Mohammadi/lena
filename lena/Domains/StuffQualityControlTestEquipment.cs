using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class StuffQualityControlTestEquipment : IEntity
  {
    protected internal StuffQualityControlTestEquipment()
    {
    }
    public int StuffId { get; set; }
    public long QualityControlTestId { get; set; }
    public long QualityControlTestEquipmentQualityControlTestId { get; set; }
    public int QualityControlEquipmentTestEquipmentId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual StuffQualityControlTest StuffQualityControlTest { get; set; }
    public virtual QualityControlTestEquipment QualityControlTestEquipment { get; set; }
  }
}