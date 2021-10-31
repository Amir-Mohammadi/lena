using System;


using lena.Domains.Enums;
namespace lena.Models.Supplies.ProvisionersCart
{
  public class FullProvisionersCartReportResult
  {
    public int Id { get; set; }
    public string Description { get; set; }
    public Nullable<int> ResponsibleEmployeeId { get; set; }
    public string RegisterEmployeeFullName { get; set; }
    public Nullable<System.DateTime> RegisterDateTime { get; set; }
    public Nullable<System.DateTime> ReportDate { get; set; }
    public Nullable<lena.Domains.Enums.ProvisionersCartStatus> Status { get; set; }
    public Nullable<int> SupplierId { get; set; }
    public byte[] RowVersion { get; set; }
    public string SupplierName { get; set; }
    public string PurchaseRequestStuffCode { get; set; }
    public string PurchaseRequestUnitName { get; set; }
    public string PurchaseRequestStuffName { get; set; }
    public string ProviderName { get; set; }
    public Nullable<double> RequestQty { get; set; }
    public Nullable<double> SuppliedQty { get; set; }
    public Nullable<int> UnitPrice { get; set; }
    public int CurrencyId { get; set; }
    public string DetailDescription { get; set; }
    public Nullable<int> ProviderId { get; set; }
    public string DetailProviderName { get; set; }
    public Nullable<lena.Domains.Enums.ProvisionersCartItemStatus> ItemStatus { get; set; }

    public Nullable<double> SupplyQty { get; set; }
    public Nullable<System.DateTime> DateTime { get; set; }
    public Nullable<int> ProvisionersCartItemId { get; set; }
  }
}