using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class AssetTransferRequest : IEntity
  {
    protected internal AssetTransferRequest()
    {
    }
    public int Id { get; set; }
    public int AssetId { get; set; }
    public Nullable<int> NewEmployeeId { get; set; }
    public Nullable<short> NewDepartmentId { get; set; }
    public int RequestingUserId { get; set; }
    public Nullable<int> ConfirmerUserId { get; set; }
    public DateTime RequestDateTime { get; set; }
    public Nullable<DateTime> ConfirmDateTime { get; set; }
    public string Description { get; set; }
    public AssetTransferRequestStatus Status { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Employee NewEmployee { get; set; }
    public virtual User RequestingUser { get; set; }
    public virtual User ConfirmerUser { get; set; }
    public virtual Department NewDepartment { get; set; }
    public virtual Asset Asset { get; set; }
  }
}