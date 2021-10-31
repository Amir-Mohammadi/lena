using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class OrderItemChangeConfirmation : BaseEntity, IEntity
  {
    protected internal OrderItemChangeConfirmation()
    {
    }
    public int OrderItemChangeRequestId { get; set; }
    public bool Confirmed { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual OrderItemChangeRequest OrderItemChangeRequest { get; set; }
  }
}
