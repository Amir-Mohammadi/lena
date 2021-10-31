using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class Cooperator : IEntity
  {
    protected internal Cooperator()
    {
      this.StoreReceipts = new HashSet<StoreReceipt>();
      this.InboundCargoCooperators = new HashSet<InboundCargoCooperator>();
      this.SerialProfiles = new HashSet<SerialProfile>();
      this.BaseEntityDocuments = new HashSet<BaseEntityDocument>();
      this.ExitReceiptRequests = new HashSet<ExitReceiptRequest>();
      this.Receipts = new HashSet<Receipt>();
      this.CooperatorFinancialAccount = new HashSet<CooperatorFinancialAccount>();
      this.ExitReceipts = new HashSet<ExitReceipt>();
      this.Finances = new HashSet<Finance>();
      this.FinanceItems = new HashSet<FinanceItem>();
      this.FinanceItemAllocationSummaries = new HashSet<FinanceItemAllocationSummary>();
      this.PriceInquries = new HashSet<PriceInquiry>();
      this.Contacts = new HashSet<Contact>();
      #region Customers
      this.DetailedCodeConfirmationRequests = new HashSet<DetailedCodeConfirmationRequest>();
      this.ProjectHeaders = new HashSet<ProjectHeader>();
      this.Orders = new HashSet<Order>();
      this.CustomerComplaints = new HashSet<CustomerComplaint>();
      this.LinkSerials = new HashSet<LinkSerial>();
      this.CustomerStuffs = new HashSet<CustomerStuff>();
      this.PriceAnnunciations = new HashSet<PriceAnnunciation>();
      this.ProjectERPs = new HashSet<ProjectERP>();
      #endregion
      #region Providers
      this.StuffProviders = new HashSet<StuffProvider>();
      this.ProviderHowToBuys = new HashSet<ProviderHowToBuy>();
      this.PurchaseOrders = new HashSet<PurchaseOrder>();
      this.BankOrders = new HashSet<BankOrder>();
      this.ProvisionersCartItems = new HashSet<ProvisionersCartItem>();
      this.ProvisionersCartItemDetails = new HashSet<ProvisionersCartItemDetail>();
      this.DetailedCodeConfirmationRequests = new HashSet<DetailedCodeConfirmationRequest>();
      #endregion
      #region Exchange
      this.CurrencyRates = new HashSet<CurrencyRate>();
      #endregion
    }
    public int Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public short CityId { get; set; }
    public CooperatorType CooperatorType { get; set; }
    public ProviderType ProviderType { get; set; }
    public string DetailedCode { get; set; } // کد تفصیلی
    public bool ConfirmationDetailedCode { get; set; } // تایید کد تفصیلی
    public byte[] RowVersion { get; set; }
    public virtual City City { get; set; }
    public virtual ICollection<Contact> CooperatorContacts { get; set; }
    public virtual ICollection<StoreReceipt> StoreReceipts { get; set; }
    public virtual ICollection<InboundCargoCooperator> InboundCargoCooperators { get; set; }
    public virtual ICollection<SerialProfile> SerialProfiles { get; set; }
    public virtual ICollection<BaseEntityDocument> BaseEntityDocuments { get; set; }
    public virtual ICollection<ExitReceiptRequest> ExitReceiptRequests { get; set; }
    public virtual ICollection<Receipt> Receipts { get; set; }
    public virtual ICollection<CooperatorFinancialAccount> CooperatorFinancialAccount { get; set; }
    public virtual ICollection<ExitReceipt> ExitReceipts { get; set; }
    public virtual ICollection<Finance> Finances { get; set; }
    public virtual ICollection<FinanceItem> FinanceItems { get; set; }
    public virtual ICollection<FinanceItemAllocationSummary> FinanceItemAllocationSummaries { get; set; }
    public virtual ICollection<PriceInquiry> PriceInquries { get; set; }
    public virtual ICollection<Contact> Contacts { get; set; }
    #region Customers
    public virtual ICollection<DetailedCodeConfirmationRequest> DetailedCodeConfirmationRequests { get; set; }
    public virtual ICollection<ProjectHeader> ProjectHeaders { get; set; }
    public virtual ICollection<Order> Orders { get; set; }
    public virtual ICollection<CustomerComplaint> CustomerComplaints { get; set; }
    public virtual ICollection<LinkSerial> LinkSerials { get; set; }
    public virtual ICollection<CustomerStuff> CustomerStuffs { get; set; }
    public virtual ICollection<PriceAnnunciation> PriceAnnunciations { get; set; }
    public virtual ICollection<ProjectERP> ProjectERPs { get; set; }
    #endregion
    #region Providers
    public virtual ICollection<StuffProvider> StuffProviders { get; set; }
    public virtual ICollection<ProviderHowToBuy> ProviderHowToBuys { get; set; }
    public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; }
    public virtual ICollection<BankOrder> BankOrders { get; set; }
    public virtual ICollection<ManualTransaction> ManualTransactions { get; set; }
    public virtual ICollection<ProvisionersCartItem> ProvisionersCartItems { get; set; }
    public virtual ICollection<ProvisionersCartItemDetail> ProvisionersCartItemDetails { get; set; }
    #endregion
    #region Exchange
    public virtual ICollection<CurrencyRate> CurrencyRates { get; set; }
    #endregion
  }
}