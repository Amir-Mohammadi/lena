using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class WarehouseExitReceiptType : IEntity
  {
    public short WarehouseId { get; set; }
    public int ExitReceiptRequestTypeId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Warehouse Warehouse { get; set; }
    public virtual ExitReceiptRequestType ExitReceiptRequestType { get; set; }
  }
}