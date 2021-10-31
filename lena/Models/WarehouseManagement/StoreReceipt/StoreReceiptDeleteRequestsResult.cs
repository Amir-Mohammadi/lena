using System;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StoreReceipt
{
  public class StoreReceiptDeleteRequestResult
  {
    public int Id { get; set; }
    public int StoreReceiptId { set; get; }
    public string StoreReceiptCode { get; set; }
    public string StuffName { get; set; }
    public string StuffCode { get; set; }
    public string CooperatorName { get; set; }
    public string CreatorUserFullName { set; get; }
    public string ChangeStatusUserFullName { set; get; }
    public double StoreReceiptAmount { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public DateTime DateTime { get; set; }
    public DateTime? ChangeStatusDateTime { get; set; }
    public DateTime? ReceiptDateTime { get; set; }
    public int? ChangeStatusUserId { set; get; }
    public int UserId { set; get; }
    public int CreatorUserId { set; get; }
    public string Description { set; get; }
    public StoreReceiptDeleteRequestStatus Status { set; get; }
    public StoreReceiptType StoreReceiptType { set; get; }
    public byte[] RowVersion { get; set; }

  }
}