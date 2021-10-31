using lena.Domains.Enums;
using System;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.ExitReceiptDeleteRequestConfirmationLog
{
  public class ExitReceiptDeleteRequestConfirmationLogResult
  {
    public int? Id { get; set; }
    public int? ExitReceiptDeleteRequestId { get; set; }
    public int? ConfirmerUserId { get; set; }
    public string ConfirmerUserFullName { get; set; }
    public DateTime? DateTime { get; set; }
    public string Description { get; set; }
    public ExitReceiptDeleteRequestStatus? Status { get; set; }
    public byte[] RowVersion { get; set; }

  }
}
