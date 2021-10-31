using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class WarehouseStoreReceiptType : IEntity
  {
    public short WarehouseId { get; set; }
    public StoreReceiptType StoreReceiptType { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Warehouse Warehouse { get; set; }
  }
}