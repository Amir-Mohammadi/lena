using lena.Domains.Enums;
using System;
using lena.Domains.Enums;
namespace lena.Models.Accounting.CostCenter
{
  public class CostCenterResult
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public CostCenterStatus Status { get; set; }
    public DateTime? ConfirmDateTime { get; set; }
    public int? ConfirmerUserId { get; set; }
    public string ConfirmerEmployeeFullName { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
  }
}