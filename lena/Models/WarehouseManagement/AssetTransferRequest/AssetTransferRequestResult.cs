using System;
using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models
{
  public class AssetTransferRequestResult
  {
    public int Id { get; set; }
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public int AssetId { get; set; }
    public string AssetCode { get; set; }
    public int? NewEmployeeId { get; set; }
    public string NewEmployeeName { get; set; }
    public short? NewDepartmentId { get; set; }
    public string NewDepartmentName { get; set; }
    public int RequestingUserId { get; set; }
    public string RequestingUserFullName { get; set; }
    public DateTime RequestDateTime { get; set; }
    public int? ConfirmerUserId { get; set; }
    public string ConfirmerUserFullName { get; set; }
    public DateTime? ConfirmDateTime { get; set; }
    public string Description { get; set; }
    public AssetTransferRequestStatus Status { get; set; }
  }
}
