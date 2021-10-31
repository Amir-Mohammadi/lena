using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class BaseTransaction : IEntity
  {
    protected internal BaseTransaction()
    {
      this.ReferencedTransactions = new HashSet<BaseTransaction>();
    }
    public int Id { get; set; }
    public double Amount { get; set; }
    public DateTime EffectDateTime { get; set; }
    public string Description { get; set; }
    public Nullable<long> StuffSerialCode { get; set; }
    public int StuffId { get; set; }
    public byte UnitId { get; set; }
    public int TransactionBatchId { get; set; }
    public short TransactionTypeId { get; set; }
    public Nullable<short> BillOfMaterialVersion { get; set; }
    public Nullable<int> ReferenceTransactionId { get; set; }
    public Nullable<short> WarehouseId { get; set; }
    public short WarehouseFiscalPeriodId { get; set; }
    public Nullable<bool> IsEstimated { get; set; }
    public bool IsDelete { get; set; }
    public DateTime? DeletedAt { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Warehouse Warehouse { get; set; }
    public virtual SerialBuffer SerialBuffer { get; set; }
    public virtual WarehouseFiscalPeriod WarehouseFiscalPeriod { get; set; }
    public virtual StuffSerial StuffSerial { get; set; }
    public virtual Unit Unit { get; set; }
    public virtual TransactionType TransactionType { get; set; }
    public virtual TransactionBatch TransactionBatch { get; set; }
    public virtual BillOfMaterial BillOfMaterial { get; set; }
    public virtual ICollection<BaseTransaction> ReferencedTransactions { get; set; }
    public virtual BaseTransaction ReferenceTransaction { get; set; }
    public virtual Stuff Stuff { get; set; }
  }
}