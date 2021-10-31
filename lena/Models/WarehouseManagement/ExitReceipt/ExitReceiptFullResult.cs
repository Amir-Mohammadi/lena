using System;
using System.Linq;
using lena.Models.WarehouseManagement.SendProduct;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.ExitReceipt
{
  public class ExitReceiptFullResult
  {
    public int Id { get; set; }
    public int? ExitReceiptId { get; set; }
    public string Code { get; set; }
    public bool? Confirmed { get; set; }
    public int? OutboundCargoId { get; set; }
    public string OutboundCargoCode { get; set; }
    public DateTime? DateTime { get; set; }
    public DateTime? TransportDateTime { get; set; }
    public string CarNumber { get; set; }
    public IQueryable<SendProductFullResult> SendProducts { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
