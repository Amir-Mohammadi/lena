using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class WorkShift : IEntity
  {
    protected internal WorkShift()
    {
      this.DepartmentWorkShifts = new HashSet<DepartmentWorkShift>();
      this.ProductionLineWorkShifts = new HashSet<ProductionLineWorkShift>();
      this.CalendarEvents = new HashSet<CalendarEvent>();
    }
    public int Id { get; set; }
    public string Name { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<DepartmentWorkShift> DepartmentWorkShifts { get; set; }
    public virtual ICollection<ProductionLineWorkShift> ProductionLineWorkShifts { get; set; }
    public virtual ICollection<CalendarEvent> CalendarEvents { get; set; }
  }
}