using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ReturnOfSale : BaseEntity, IEntity
  {
    protected internal ReturnOfSale()
    {
      this.RepairProductions = new HashSet<RepairProduction>();
      this.ReturnOfSaleStatusLogs = new HashSet<ReturnOfSaleStatusLog>();
      this.QualityControlItems = new HashSet<QualityControlItem>();
    }
    public ReturnOfSaleType Type { get; set; }
    public double Qty { get; set; }
    public byte UnitId { get; set; }
    public ReturnOfSaleStatus Status { get; set; }
    public int ReturnStoreReceiptId { get; set; }
    public Nullable<long> StuffSerialCode { get; set; }
    public int StuffId { get; set; }
    public Nullable<int> SendProductId { get; set; }
    public Nullable<int> MainStuffId { get; set; }
    public string ExitReceiptCode { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<RepairProduction> RepairProductions { get; set; }
    public virtual ReturnStoreReceipt ReturnStoreReceipt { get; set; }
    public virtual StuffSerial StuffSerial { get; set; }
    public virtual Stuff Stuff { get; set; }
    public virtual SendProduct SendProduct { get; set; }
    public virtual Unit Unit { get; set; }
    public virtual ICollection<ReturnOfSaleStatusLog> ReturnOfSaleStatusLogs { get; set; }
    public virtual ICollection<QualityControlItem> QualityControlItems { get; set; }
    public virtual ReturnOfSaleSummary ReturnOfSaleSummary { get; set; }
  }
}