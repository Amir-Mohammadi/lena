using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class Stuff : IEntity
  {
    protected internal Stuff()
    {
      this.BillOfMaterialDetails = new HashSet<BillOfMaterialDetail>();
      this.EquivalentStuffDetails = new HashSet<EquivalentStuffDetail>();
      this.OrderItems = new HashSet<OrderItem>();
      this.StuffSerials = new HashSet<StuffSerial>();
      this.StockCheckingTags = new HashSet<StockCheckingTag>();
      this.StuffProviders = new HashSet<StuffProvider>();
      this.BillOfMaterials = new HashSet<BillOfMaterial>();
      this.PurchaseRequests = new HashSet<PurchaseRequest>();
      this.PurchaseOrders = new HashSet<PurchaseOrder>();
      this.StoreReceipts = new HashSet<StoreReceipt>();
      this.StuffRequestItems = new HashSet<StuffRequestItem>();
      this.WarehouseIssueItems = new HashSet<WarehouseIssueItem>();
      this.ProductionStuffDetails = new HashSet<ProductionStuffDetail>();
      this.SerialProfiles = new HashSet<SerialProfile>();
      this.PreparingSendingItems = new HashSet<PreparingSendingItem>();
      this.StuffQualityControlTests = new HashSet<StuffQualityControlTest>();
      this.QualityControlItems = new HashSet<QualityControlItem>();
      this.QualityControls = new HashSet<QualityControl>();
      this.StuffPrices = new HashSet<StuffPrice>();
      this.BaseTransactions = new HashSet<BaseTransaction>();
      this.ResponseStuffRequestItems = new HashSet<ResponseStuffRequestItem>();
      this.StuffDocuments = new HashSet<StuffDocument>();
      this.SuppliesPurchaserUsers = new HashSet<SuppliesPurchaserUser>();
      this.ExitReceiptRequests = new HashSet<ExitReceiptRequest>();
      this.QtyCorrectionRequests = new HashSet<QtyCorrectionRequest>();
      this.StuffProductionFaultTypes = new HashSet<StuffProductionFaultType>();
      this.StuffStockPlaces = new HashSet<StuffStockPlace>();
      this.StuffRequestMilestoneDetails = new HashSet<StuffRequestMilestoneDetail>();
      this.Decompositions = new HashSet<Decomposition>();
      this.ReturnOfSales = new HashSet<ReturnOfSale>();
      this.StockCheckingStuffs = new HashSet<StockCheckingStuff>();
      this.ProductionLineEmployeeIntervals = new HashSet<ProductionLineEmployeeInterval>();
      this.GeneralStuffRequests = new HashSet<GeneralStuffRequest>();
      this.CustomerStuffs = new HashSet<CustomerStuff>();
      this.StuffFractionTemporaryStuffs = new HashSet<StuffFractionTemporaryStuff>();
      this.BillOfMaterialPriceHistories = new HashSet<BillOfMaterialPriceHistory>();
      this.StuffQualityControlObservations = new HashSet<StuffQualityControlObservation>();
      this.PriceAnnunciationItems = new HashSet<PriceAnnunciationItem>();
      this.StuffAssets = new HashSet<Asset>();
      this.ProjectERPs = new HashSet<ProjectERP>();
      this.PriceInquries = new HashSet<PriceInquiry>();
    }
    public int Id { get; set; }
    public string Name { get; set; }
    public string Noun { get; set; }
    public string Title { get; set; }
    public string Code { get; set; }
    public bool IsActive { get; set; }
    public string Description { get; set; }
    public short StuffCategoryId { get; set; }
    public byte UnitTypeId { get; set; }
    public StuffType StuffType { get; set; }
    public int StockSafety { get; set; }
    public double FaultyPercentage { get; set; }
    public bool NeedToQualityControl { get; set; }
    public bool NeedToQualityControlDocumentUpload { get; set; }
    public bool IsTraceable { get; set; }
    public Nullable<double> GrossWeight { get; set; }
    public Nullable<double> Volume { get; set; }
    public byte[] RowVersion { get; set; }
    public Nullable<short> QualityControlDepartmentId { get; set; }
    public Nullable<int> QualityControlEmployeeId { get; set; }
    public Nullable<int> StuffHSGroupId { get; set; }
    public double Tolerance { get; set; }
    public Nullable<double> NetWeight { get; set; }
    public Nullable<int> QualityControlCheckDuration { get; set; }
    public int? StuffPurchaseCategoryId { get; set; }
    public short CeofficientSet { get; set; }
    public int? StuffDefinitionRequestId { get; set; }
    public int? ProjectHeaderId { get; set; }
    public StuffPurchaseCategory StuffPurchaseCategory { get; set; }
    //public int? StuffDefinitionRequestId { get; set; }
    public StuffDefinitionRequest StuffDefinitionRequest { get; set; }
    public virtual StuffCategory StuffCategory { get; set; }
    public virtual UnitType UnitType { get; set; }
    public virtual ICollection<BillOfMaterialDetail> BillOfMaterialDetails { get; set; }
    public virtual ICollection<EquivalentStuffDetail> EquivalentStuffDetails { get; set; }
    public virtual ICollection<OrderItem> OrderItems { get; set; }
    public virtual ICollection<StuffSerial> StuffSerials { get; set; }
    public virtual ICollection<StockCheckingTag> StockCheckingTags { get; set; }
    public virtual ProjectHeader ProjectHeader { get; set; }
    public virtual ICollection<StuffProvider> StuffProviders { get; set; }
    public virtual ICollection<BillOfMaterial> BillOfMaterials { get; set; }
    public virtual ICollection<PurchaseRequest> PurchaseRequests { get; set; }
    public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; }
    public virtual ICollection<StoreReceipt> StoreReceipts { get; set; }
    public virtual ICollection<StuffRequestItem> StuffRequestItems { get; set; }
    public virtual ICollection<WarehouseIssueItem> WarehouseIssueItems { get; set; }
    public virtual ICollection<ProductionStuffDetail> ProductionStuffDetails { get; set; }
    public virtual ICollection<SerialProfile> SerialProfiles { get; set; }
    public virtual ICollection<PreparingSendingItem> PreparingSendingItems { get; set; }
    public virtual ICollection<StuffQualityControlTest> StuffQualityControlTests { get; set; }
    public virtual ICollection<QualityControlItem> QualityControlItems { get; set; }
    public virtual ICollection<QualityControl> QualityControls { get; set; }
    public virtual ICollection<StuffPrice> StuffPrices { get; set; }
    public virtual ICollection<BaseTransaction> BaseTransactions { get; set; }
    public virtual Department QualityControlDepartment { get; set; }
    public virtual Employee QualityControlEmployee { get; set; }
    public virtual ICollection<ResponseStuffRequestItem> ResponseStuffRequestItems { get; set; }
    public virtual ICollection<StuffDocument> StuffDocuments { get; set; }
    public virtual ICollection<SuppliesPurchaserUser> SuppliesPurchaserUsers { get; set; }
    public virtual ICollection<ExitReceiptRequest> ExitReceiptRequests { get; set; }
    public virtual ICollection<QtyCorrectionRequest> QtyCorrectionRequests { get; set; }
    public virtual ICollection<StuffProductionFaultType> StuffProductionFaultTypes { get; set; }
    public virtual ICollection<StuffStockPlace> StuffStockPlaces { get; set; }
    public virtual ICollection<StuffRequestMilestoneDetail> StuffRequestMilestoneDetails { get; set; }
    public virtual ICollection<Decomposition> Decompositions { get; set; }
    public virtual StuffHSGroup StuffHSGroup { get; set; }
    public virtual ICollection<ReturnOfSale> ReturnOfSales { get; set; }
    public virtual ICollection<StockCheckingStuff> StockCheckingStuffs { get; set; }
    public virtual ICollection<ManualTransaction> ManualTransactions { get; set; }
    public virtual ICollection<ProductionLineEmployeeInterval> ProductionLineEmployeeIntervals { get; set; }
    public virtual ICollection<StuffDefinitionRequest> StuffDefinitionRequests { get; set; }
    public virtual ICollection<GeneralStuffRequest> GeneralStuffRequests { get; set; }
    public virtual ICollection<CustomerStuff> CustomerStuffs { get; set; }
    public virtual ICollection<StuffFractionTemporaryStuff> StuffFractionTemporaryStuffs { get; set; }
    public virtual ICollection<BillOfMaterialPriceHistory> BillOfMaterialPriceHistories { get; set; }
    public virtual ICollection<StuffQualityControlObservation> StuffQualityControlObservations { get; set; }
    public virtual ICollection<PriceAnnunciationItem> PriceAnnunciationItems { get; set; }
    public virtual ICollection<ProjectERP> ProjectERPs { get; set; }
    public virtual ICollection<Asset> StuffAssets { get; set; }
    public virtual ICollection<PriceInquiry> PriceInquries { get; set; }
  }
}