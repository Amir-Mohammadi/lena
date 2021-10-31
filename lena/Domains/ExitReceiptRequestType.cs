using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ExitReceiptRequestType : IEntity
  {
    protected internal ExitReceiptRequestType()
    {
      this.ExitReceiptRequests = new HashSet<ExitReceiptRequest>();
      this.WarehouseExitReceiptTypes = new HashSet<WarehouseExitReceiptType>();
    }
    public int Id { get; set; }
    public string Title { get; set; }
    public bool AutoConfirm { get; set; }
    public bool IsActive { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<ExitReceiptRequest> ExitReceiptRequests { get; set; }
    public virtual ICollection<WarehouseExitReceiptType> WarehouseExitReceiptTypes { get; set; }
  }
}
