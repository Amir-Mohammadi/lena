using System;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.SendProduct
{
  public class SendProductResult
  {
    public int? Id { get; set; }
    public string Code { get; set; }
    public int StuffId { get; set; }
    public int? PriceAnnunciationItemId { get; set; }
    public double? PriceAnnunciation { get; set; }
    public string CurrencyName { get; set; }

    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public double Qty { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public int? CooperatorId { set; get; }
    public string CooperatorCode { set; get; }
    public string CooperatorName { set; get; }
    public int ExitReceiptRequestId { set; get; }
    public string ExitReceiptRequestCode { set; get; }
    public string Address { set; get; }
    public string Description { set; get; }
    public byte[] RowVersion { get; set; }
    public DateTime DateTime { get; set; }
    public DateTime? TransportDateTime { get; set; }

  }
}
