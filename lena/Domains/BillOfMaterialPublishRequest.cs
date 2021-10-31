using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class BillOfMaterialPublishRequest : BaseEntity, IEntity
  {
    protected internal BillOfMaterialPublishRequest()
    {
    }
    public short BillOfMaterialVersion { get; set; }
    public int BillOfMaterialStuffId { get; set; }
    public BillOfMaterialPublishRequestStatus Status { get; set; }
    public BillOfMaterialPublishRequestType Type { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual BillOfMaterial BillOfMaterial { get; set; }
  }
}