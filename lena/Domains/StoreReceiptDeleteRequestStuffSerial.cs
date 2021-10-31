using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class StoreReceiptDeleteRequestStuffSerial : IEntity
  {
    public int Id { get; set; }
    public int StoreReceiptDeleteRequestId { get; set; }
    public int StuffSerialId { get; set; }
    public long StuffSerialCode { get; set; }
    public double Amount { get; set; }
    public byte UnitId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual StoreReceiptDeleteRequest StoreReceiptDeleteRequest { get; set; }
    public virtual StuffSerial StuffSerial { get; set; }
    public virtual Unit Unit { get; set; }
  }
}