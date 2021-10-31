using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class AssetLog : IEntity
  {
    protected internal AssetLog()
    {
    }
    public int Id { get; set; }
    public int AssetId { get; set; }
    public Nullable<int> EmployeeId { get; set; }
    public short DepartmentId { get; set; }
    public int UserId { get; set; }
    public DateTime CreateDateTime { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Asset Asset { get; set; }
    public virtual Employee Employee { get; set; }
    public virtual User User { get; set; }
    public virtual Department Department { get; set; }
  }
}