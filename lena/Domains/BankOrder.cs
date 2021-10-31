using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class BankOrder : BaseEntity, IEntity
  {
    protected internal BankOrder()
    {
      this.Ladings = new HashSet<Lading>();
      this.BankOrderDetails = new HashSet<BankOrderDetail>();
      this.BankOrderLogs = new HashSet<BankOrderLog>();
      this.FinancialDocumentBankOrders = new HashSet<FinancialDocumentBankOrder>();
      this.Allocations = new HashSet<Allocation>();
      this.BankOrderCurrencySources = new HashSet<BankOrderCurrencySource>();
      this.BankOrderCosts = new HashSet<BankOrderCost>();
    }
    public string OrderNumber { get; set; }
    public string FolderCode { get; set; }
    public int StuffPriority { get; set; } // اولویت کالایی
    public DateTime RegisterDate { get; set; }
    public DateTime ExpireDate { get; set; }
    public Nullable<DateTime> SettlementDateTime { get; set; } // تاریخ تسویه
    public int ProviderId { get; set; }
    public byte CurrencyId { get; set; }
    public BankOrderStatus Status { get; set; }
    public BankOrderType BankOrderType { get; set; }
    public byte BankId { get; set; }
    public short CustomhouseId { get; set; }
    public byte CountryId { get; set; }
    public double TransferCost { get; set; }
    public double FOB { get; set; } //free on board
    public double TotalAmount { get; set; } // FOB + TransferCost = BankOrderAmount
    public short BankOrderContractTypeId { get; set; }
    public bool WithoutCurrencyTransfer { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<Lading> Ladings { get; set; }
    public virtual ICollection<BankOrderDetail> BankOrderDetails { get; set; }
    public virtual Cooperator Provider { get; set; }
    public virtual Currency Currency { get; set; }
    public virtual Bank Bank { get; set; }
    public virtual Customhouse Customhouse { get; set; }
    public virtual Country Country { get; set; }
    public virtual BankOrderContractType BankOrderContractType { get; set; }
    public virtual ICollection<BankOrderLog> BankOrderLogs { get; set; }
    public virtual BankOrderLog CurrentBankOrderLog { get; set; }
    public virtual ICollection<FinancialDocumentBankOrder> FinancialDocumentBankOrders { get; set; }
    public virtual Enactment Enactment { get; set; }
    public virtual ICollection<Allocation> Allocations { get; set; }
    public virtual ICollection<BankOrderCurrencySource> BankOrderCurrencySources { get; set; }
    public virtual ICollection<BankOrderCost> BankOrderCosts { get; set; }
  }
}