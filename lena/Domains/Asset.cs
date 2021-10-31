using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class Asset : IEntity
  {
    protected internal Asset()
    {
      this.AssetLogs = new HashSet<AssetLog>();
      this.AssetTransferRequests = new HashSet<AssetTransferRequest>();
    }
    public int Id { get; set; }
    public string Code { get; set; }
    public int StuffId { get; set; }
    public long StuffSerialCode { get; set; }
    public Nullable<int> EmployeeId { get; set; }
    public short DepartmentId { get; set; }
    public int UserId { get; set; }
    public short WarehouseId { get; set; }
    public DateTime CreateDateTime { get; set; }
    public string Description { get; set; }
    public AssetStatus Status { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Stuff Stuff { get; set; }
    public virtual Employee Employee { get; set; }
    public virtual User User { get; set; }
    public virtual Department Department { get; set; }
    public virtual Warehouse Warehouse { get; set; }
    public virtual ICollection<AssetLog> AssetLogs { get; set; }
    public virtual ICollection<AssetTransferRequest> AssetTransferRequests { get; set; }
    public virtual StuffSerial StuffSerial { get; set; }

  }
}