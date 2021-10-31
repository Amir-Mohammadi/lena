using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class IndicatorWeight : IEntity
  {
    // وزن های مربوط به هر شاخص
    protected internal IndicatorWeight()
    { }
    public int Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public byte[] RowVersion { get; set; }
    public short? DepartmentId { get; set; }
    public Department Department { get; set; }
    public virtual ICollection<WeightDay> WeightDays { get; set; }
  }
}
