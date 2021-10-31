using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class PurchaseStep : BaseEntity, IEntity
  {
    protected internal PurchaseStep()
    {
    }
    public int HowToBuyDetailId { get; set; }
    public DateTime FollowUpDateTime { get; set; }
    public bool IsCurrentStep { get; set; }
    public int CargoItemId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual HowToBuyDetail HowToBuyDetail { get; set; }
    public virtual CargoItem CargoItem { get; set; }
  }
}