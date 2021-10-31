using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class DepartmentWorkShift : IEntity
  {
    protected internal DepartmentWorkShift()
    {
    }
    public int WorkShiftId { get; set; }
    public short DepartmentId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Department Department { get; set; }
    public virtual WorkShift WorkShift { get; set; }
  }
}
