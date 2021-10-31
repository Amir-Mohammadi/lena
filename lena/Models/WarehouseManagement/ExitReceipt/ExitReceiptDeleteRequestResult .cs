using System;
using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.ExitReceipt
{
  public class ExitReceiptDeleteRequestResult
  {
    public int Id { get; set; }
    public int ExitReceiptId { set; get; }
    public string ExitReceiptCode { set; get; }
    public string ExitReceiptFullName { set; get; }
    public string CreatorUserFullName { set; get; }
    public string ChangeStatusUserFullName { set; get; }
    public DateTime CreateDateTime { get; set; }
    public DateTime? ChangeStatusDateTime { get; set; }
    public int CreatorUserId { set; get; }
    public string Description { set; get; }
    public ExitReceiptDeleteRequestStatus? Status { set; get; }
    public byte[] RowVersion { get; set; }

  }
}
