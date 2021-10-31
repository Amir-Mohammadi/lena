using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ProductionLineEmployeeInterval : IEntity
  {
    protected internal ProductionLineEmployeeInterval()
    {
      this.ProductionLineEmployeeIntervalDetails = new HashSet<ProductionLineEmployeeIntervalDetail>();
    }
    public int Id { get; set; }
    public string RFId { get; set; }
    public int ProductionLineId { get; set; }
    public int EmployeeId { get; set; }
    public int StuffId { get; set; }
    public string HashedOperation { get; set; }
    public DateTime EntranceDateTime { get; set; }
    public Nullable<DateTime> ExitDateTime { get; set; }
    public Nullable<DateTime> LastMoidfied { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ProductionLine ProductionLine { get; set; }
    public virtual Employee Employee { get; set; }
    public virtual Stuff Stuff { get; set; }
    public virtual ICollection<ProductionLineEmployeeIntervalDetail> ProductionLineEmployeeIntervalDetails { get; set; }
  }
}
