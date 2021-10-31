using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class TestEquipment : IEntity
  {
    protected internal TestEquipment()
    {
    }
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<QualityControlTestEquipment> QualityControlTestEquipments { get; set; }
  }
}