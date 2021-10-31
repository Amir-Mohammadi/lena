using System;
using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.SaleManagement.SendPermission
{
  public class SendPermissionComboResult
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public double Qty { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public DateTime DateTime { get; set; }
    public SendPermissionStatusType SendPermissionStatusType { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
