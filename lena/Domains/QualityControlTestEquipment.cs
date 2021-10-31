using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class QualityControlTestEquipment : IEntity
  {
    protected internal QualityControlTestEquipment()
    {
      this.StuffQualityControlTests = new HashSet<StuffQualityControlTest>();
    }
    public long QualityControlTestId { get; set; }
    public int TestEquipmentId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual TestEquipment TestEquipment { get; set; }
    public virtual QualityControlTest QualityControlTest { get; set; }
    public ICollection<StuffQualityControlTestEquipment> StuffQualityControlTestEquipments { get; set; }
    public virtual ICollection<StuffQualityControlTest> StuffQualityControlTests { get; set; }
  }
}