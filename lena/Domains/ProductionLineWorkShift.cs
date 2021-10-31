using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ProductionLineWorkShift : IEntity
  {
    protected internal ProductionLineWorkShift()
    {
    }
    public int Id { get; set; }
    public int WorkShiftId { get; set; }
    public int ProductionLineId { get; set; }
    public DateTime SaveDate { get; set; }
    public DateTime FromDate { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ProductionLine ProductionLine { get; set; }
    public virtual WorkShift WorkShift { get; set; }
  }
}
