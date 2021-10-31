using lena.Domains.Enums;
using System;


using lena.Domains.Enums;
namespace lena.Models.Supplies.PurchaseReport
{
  public class PurchaseReportResult
  {
    public string PurchaseRequestCode { get; set; }
    public string StuffName { get; set; }
    public string StuffNoun { get; set; }
    public int? StuffId { get; set; }
    public string StuffCode { get; set; }
    public string PurchaseRequestUnit { get; set; }
    public PurchaseRequestStatus? PurchaseRequestStatus { get; set; }
    public double PurchaseRequestQty { get; set; }
    public double OrderedQty { get; set; }
    public double CargoedQty { get; set; }
    public double ReceiptedQty { get; set; }
    public int PurchaseRequestId { get; set; }
    public string OrderCode { get; set; }
    public string CargoCode { get; set; }
    public CargoItemStatus? CargoItemStatus { get; set; }
    public int? InboundNumber { get; set; }
    public int? ReceiptNumber { get; set; }
    public string ProviderCode { get; set; }
    public string ProviderName { get; set; }
    public string OrderUnit { get; set; }
    public DateTime? PurchaseRequestDate { get; set; }
    public DateTime? PurchaseDeadline { get; set; }
    public DateTime? OrderDate { get; set; }
    public DateTime? EstimatedCargoRecieveDateTime { get; set; }
    public DateTime? CargoItemSendDate { get; set; }
    public DateTime? CargoItemDateTime { get; set; }
    public DateTime? LadingDateTime { get; set; }
    public DateTime? DeliverToCustomsDate { get; set; }
    public DateTime? DeliverToCompanyDate { get; set; }
    public DateTime? ShoppingDate { get; set; }
    public DateTime? QualityControledDate { get; set; }

    public DateTime? CurrentLadingBankOrderLogDate { get; set; }
    public DateTime? CurrentLadingCustomeLogDate { get; set; }
    public string CurrentLadingBankOrderLogName { get; set; }

    public string CurrentLadingCustomehouseLogName { get; set; }
    public int? QualityControlObserver { get; set; }
    public string Description { get; set; }
    public int? PurchaseRequestUserId { get; set; }
    public string PurchaseRequestUserName { get; set; }
    public string PurchaseRequestEmployeeCode { get; set; }
    public string PurchaseRequestEmployeeName { get; set; }
    public string PurchaseRequestEmployeeDepartmentName { get; set; }
    public int? Orderer { get; set; }
    public int? PendingCargoSubmitUser { get; set; }
    public int? TemporaryReceiptUser { get; set; }
    public int? QualityControlAccepter { get; set; }
    public int? QualityControlUserId { get; set; }
    public double QualityControlPassedQty { get; set; }
    public int? ReceiptUserId { get; set; }
    public string ReceiptUserName { get; set; }
    public string OrdererName { get; set; }
    public byte[] RowVersion { get; set; }

    public string LadingCode { get; set; }
    public string CurrentBankOrderStatusName { get; set; }
    public QualityControlStatus? QualityControlStatus { get; set; }
    public PurchaseOrderStatus? PurchaseOrderStatus { get; set; }
    public LadingItemStatus? LadingItemStatus { get; set; }

    public BankOrderStatus? BankOrderStatus { get; set; }
    public int? EmployeeId { get; set; }
    public int? ForwarderId { get; set; }
    public string ForwarderName { get; set; }
    public BuyingProcess? BuyingProcess { get; set; }
    public Nullable<double> PurchaseOrderPrice { get; set; }
    public string PurchaseOrderCurrencyTitle { get; set; }
    public Nullable<byte> PurchaseOrderCurrencyId { get; set; }
  }
}
