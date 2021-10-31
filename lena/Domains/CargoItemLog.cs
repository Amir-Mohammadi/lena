using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class CargoItemLog : IEntity
  {
    public int Id { get; set; }
    public int ModifierUserId { get; set; }
    public DateTime ModifyDateTime { get; set; }
    public int CargoItemId { get; set; }
    public string CargoItemCode { get; set; }
    public bool IsDelete { get; set; }
    public DateTime? DeletedAt { get; set; }
    public double NewCargoItemQty { get; set; }
    public double OldCargoItemDetailQty { get; set; }
    public CargoItemLogStatus CargoItemLogStatus { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual User ModifierUser { get; set; }
    public virtual CargoItem CargoItem { get; set; }
  }
}