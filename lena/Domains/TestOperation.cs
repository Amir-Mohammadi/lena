using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class TestOperation : IEntity
  {
    protected internal TestOperation()
    {
    }
    public int Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<QualityControlTestOperation> QualityControlTestOperations { get; set; }
  }
}