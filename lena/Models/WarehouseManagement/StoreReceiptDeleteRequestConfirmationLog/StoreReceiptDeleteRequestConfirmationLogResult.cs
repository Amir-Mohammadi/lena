using lena.Domains.Enums.SortTypes;
using System;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StoreReceiptDeleteRequestStuffSerialResult
{
  public class StoreReceiptDeleteRequestConfirmationLogResult
  {
    public int? Id { get; set; }
    public int? StoreReceiptDeleteRequestId { get; set; }
    public int? ConfirmerUserId { get; set; }
    public string ConfirmerUserFullName { get; set; }
    public DateTime? DateTime { get; set; }
    public string Description { get; set; }
    public StoreReceiptDeleteRequestStatus? Status { get; set; }
    public byte[] RowVersion { get; set; }

  }
}
