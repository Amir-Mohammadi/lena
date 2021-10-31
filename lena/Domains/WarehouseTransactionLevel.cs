using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class WarehouseTransactionLevel : IEntity
  {
    public short WareHouserId { get; set; }
    public TransactionLevel TransactionLevel { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Warehouse Warehouse { get; set; }
  }
}