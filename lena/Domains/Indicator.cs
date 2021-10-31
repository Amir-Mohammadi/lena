using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class Indicator : IEntity
  {
    protected internal Indicator()
    { }
    public int Id { get; set; }
    public string Type { get; set; }
    public string Name { get; set; }
    public double Target { get; set; }
    public double Weight { get; set; }
    public int ApiInfoId { get; set; }
    public string Formula { get; set; }
    public short DepartmentId { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Department Department { get; set; }
    public virtual ApiInfo ApiInfo { get; set; }
  }
}
