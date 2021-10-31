using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public class GeneralStuffRequestDetail : IEntity
  {
    public int Id { get; set; }
    public int GeneralStuffRequestId { get; set; }
    public virtual GeneralStuffRequest GeneralStuffRequest { get; set; }
    public int? StuffRequestId { get; set; }
    public StuffRequest StuffRequest { get; set; }
    public int? PurchaseRequestId { get; set; }
    public virtual PurchaseRequest PurchaseRequest { get; set; }
    public int? AlternativePurchaseRequestId { get; set; }
    public virtual PurchaseRequest AlternativePurchaseRequest { get; set; }
    public double Qty { get; set; }
    public byte UnitId { get; set; }
    public virtual Unit Unit { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
