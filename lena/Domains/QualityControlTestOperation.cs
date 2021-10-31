using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class QualityControlTestOperation : IEntity
  {
    protected internal QualityControlTestOperation()
    {
      this.StuffQualityControlTests = new HashSet<StuffQualityControlTest>();
    }
    public long QualityControlTestId { get; set; }
    public int TestOperationId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual TestOperation TestOperation { get; set; }
    public virtual QualityControlTest QualityControlTest { get; set; }
    public ICollection<StuffQualityControlTestOperation> StuffQualityControlTestOperations { get; set; }
    public virtual ICollection<StuffQualityControlTest> StuffQualityControlTests { get; set; }
  }
}