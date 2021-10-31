using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class StockChecking : IEntity
  {
    protected internal StockChecking()
    {
      this.StockCheckingPersons = new HashSet<StockCheckingPerson>();
      this.StockCheckingWarehouses = new HashSet<StockCheckingWarehouse>();
      this.StockCheckingStuffs = new HashSet<StockCheckingStuff>();
    }
    public int Id { get; set; }
    public string Title { get; set; }
    public DateTime StartDate { get; set; }
    public Nullable<DateTime> EndDate { get; set; }
    public StockCheckingStatus Status { get; set; }
    public DateTime CreateDate { get; set; }
    public int UserId { get; set; }
    public Nullable<int> ActiveTagTypeId { get; set; }
    public byte[] RowVersion { get; set; }
    public bool ShowInventory { get; set; }
    public virtual ICollection<StockCheckingPerson> StockCheckingPersons { get; set; }
    public virtual ICollection<StockCheckingWarehouse> StockCheckingWarehouses { get; set; }
    public virtual User User { get; set; }
    public virtual TagType ActiveTagType { get; set; }
    public virtual ICollection<StockCheckingStuff> StockCheckingStuffs { get; set; }
  }
}