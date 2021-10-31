using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class User : IEntity
  {
    protected internal User()
    {
      this.LoginFailedCount = 0;
      this.StuffPriceDiscrepancies = new HashSet<StuffPriceDiscrepancy>();
      this.Memberships = new HashSet<Membership>();
      this.Permissions = new HashSet<Permission>();
      this.ScrumEntityComments = new HashSet<ScrumEntityComment>();
      this.ScrumTasks = new HashSet<ScrumTask>();
      this.ScrumEntityLogs = new HashSet<ScrumEntityLog>();
      this.Messages = new HashSet<Message>();
      this.MessageSends = new HashSet<MessageSend>();
      this.UserPosts = new HashSet<UserPost>();
      this.ToUserMessageRelations = new HashSet<UserMessageRelation>();
      this.FromUserMessageRelations = new HashSet<UserMessageRelation>();
      this.StockCheckingPersons = new HashSet<StockCheckingPerson>();
      this.TagCountings = new HashSet<TagCounting>();
      this.StockCheckings = new HashSet<StockChecking>();
      this.TransactionBatches = new HashSet<TransactionBatch>();
      this.BaseEntities = new HashSet<BaseEntity>();
      this.ReportVersions = new HashSet<ReportVersion>();
      this.ProvisionersCarts = new HashSet<ProvisionersCart>();
      this.Printers = new HashSet<Printer>();
      this.PrinterSettings = new HashSet<ReportPrintSetting>();
      this.Notifications = new HashSet<Notification>();
      this.UserSettings = new HashSet<UserSetting>();
      this.BaseEntityConfirmTypes = new HashSet<BaseEntityConfirmType>();
      this.BaseEntityConfirmations = new HashSet<BaseEntityConfirmation>();
      this.SuppliesPurchaserUsers = new HashSet<SuppliesPurchaserUser>();
      this.FinancialLimits = new HashSet<FinancialLimit>();
      this.QualityControls = new HashSet<QualityControl>();
      this.BaseEntityDocuments = new HashSet<BaseEntityDocument>();
      this.BaseEntityLogs = new HashSet<BaseEntityLog>();
      this.FinancialTransactionBatches = new HashSet<FinancialTransactionBatch>();
      this.BankOrderStateLogs = new HashSet<BankOrderLog>();
      this.BillOfMaterials = new HashSet<BillOfMaterial>();
      this.SendPermissions = new HashSet<SendPermission>();
      this.SerialProfiles = new HashSet<SerialProfile>();
      this.ConditionalQualityControls = new HashSet<ConditionalQualityControl>();
      this.StuffPrices = new HashSet<StuffPrice>();
      this.ProductionLineRepairUnits = new HashSet<ProductionLineRepairUnit>();
      this.LadingCustomhouseLogs = new HashSet<LadingCustomhouseLog>();
      this.StuffSerials = new HashSet<StuffSerial>();
      this.BillOfMaterialDocuments = new HashSet<BillOfMaterialDocument>();
      this.PurchaseOrderStepDetails = new HashSet<PurchaseOrderStepDetail>();
      this.PurchaseRequestStepDetails = new HashSet<PurchaseRequestStepDetail>();
      this.PurchaseOrderSteps = new HashSet<PurchaseOrderStep>();
      this.PurchaseRequestSteps = new HashSet<PurchaseRequestStep>();
      this.LadingBankOrderLogs = new HashSet<LadingBankOrderLog>();
      this.UserTokens = new HashSet<UserToken>();
      this.OrderDocuments = new HashSet<OrderDocument>();
      this.EnactmentActionProcessLogs = new HashSet<EnactmentActionProcessLog>();
      this.DemandantLadingChangeRequests = new HashSet<LadingChangeRequest>();
      this.ConfirmerLadingChangeRequests = new HashSet<LadingChangeRequest>();
      this.FinanceConfirmations = new HashSet<FinanceConfirmation>();
      this.Finances = new HashSet<Finance>();
      this.FinanceItems = new HashSet<FinanceItem>();
      this.FinanceItemConfirmations = new HashSet<FinanceItemConfirmation>();
      this.ReceivedFinanceItems = new HashSet<FinanceItem>();
      this.DetailedCodeConfirmationRequest = new HashSet<DetailedCodeConfirmationRequest>();
      this.ConfirmerDetailedCodeConfirmationRequest = new HashSet<DetailedCodeConfirmationRequest>();
      this.PurchaseRequestEditLogs = new HashSet<PurchaseRequestEditLog>();
      this.OrganizationPosts = new HashSet<OrganizationPost>();
      this.OrganizationJobs = new HashSet<OrganizationJob>();
      this.OrganizationPostHistories = new HashSet<OrganizationPostHistory>();
      this.FinanceAllocations = new HashSet<FinanceAllocation>();
      this.Risks = new HashSet<Risk>();
      this.RiskResolveCreator = new HashSet<RiskResolve>();
      this.RiskResolveReviewer = new HashSet<RiskResolve>();
      this.RiskStatuses = new HashSet<RiskStatus>();
      this.QualityControlConfirmationTests = new HashSet<QualityControlConfirmationTest>();
      this.QualityControlConfirmationTestItems = new HashSet<QualityControlConfirmationTestItem>();
      this.EmployeeWorkReports = new HashSet<EmployeeWorkReport>();
      this.CustomerComplaints = new HashSet<CustomerComplaint>();
      this.EmployeeEvaluations = new HashSet<EmployeeEvaluation>();
      this.EmployeeEvaluationItems = new HashSet<EmployeeEvaluationItem>();
      this.EmployeeEvaluationPeriods = new HashSet<EmployeeEvaluationPeriod>();
      this.ReviewerSerialFailedOperations = new HashSet<SerialFailedOperation>();
      this.ConfirmSerialFailedOperations = new HashSet<SerialFailedOperation>();
      this.ProductionOperatorEmployeeBans = new HashSet<ProductionOperatorEmployeeBan>();
      this.LinkSerials = new HashSet<LinkSerial>();
      this.LinkerLinkSerials = new HashSet<LinkSerial>();
      this.CustomerComplaintSummaries = new HashSet<CustomerComplaintSummary>();
      this.ProposalRecommenderUsers = new HashSet<Proposal>();
      this.Proposals = new HashSet<Proposal>();
      this.ProposalTypes = new HashSet<ProposalType>();
      this.ProposalQAReviews = new HashSet<ProposalQAReview>();
      this.ProposalReviewCommittees = new HashSet<ProposalReviewCommittee>();
      this.DepartmentManagers = new HashSet<DepartmentManager>();
      this.TestConditions = new HashSet<TestCondition>();
      this.GeneralStuffRequests = new HashSet<GeneralStuffRequest>();
      this.DemandantSerialReturnRequests = new HashSet<ReturnSerialToPreviousStateRequest>();
      this.ConfirmerSerilReturnRequests = new HashSet<ReturnSerialToPreviousStateRequest>();
      this.WrongDoerSerilReturnRequests = new HashSet<ReturnSerialToPreviousStateRequest>();
      this.ConfirmerCostCenters = new HashSet<CostCenter>();
      this.PermissionRequestRegisterars = new HashSet<PermissionRequest>();
      this.PermissionRequestConfirmators = new HashSet<PermissionRequestAction>();
      this.PermissionRequestIntenders = new HashSet<PermissionRequest>();
      this.IranKhodroSerials = new HashSet<IranKhodroSerial>();
      this.QualityControlSamples = new HashSet<QualityControlSample>();
      this.StatusChangerQualityControlSample = new HashSet<QualityControlSample>();
      this.StuffFractionReportTemporaryStuffs = new HashSet<StuffFractionTemporaryStuff>();
      this.RegistrantMinutesMeetings = new HashSet<MinutesMeeting>();
      this.SecretaryMinutesMeetings = new HashSet<MinutesMeeting>();
      this.BossMinutesMeetings = new HashSet<MinutesMeeting>();
      this.OperatorMeetingAprovals = new HashSet<MeetingApproval>();
      this.BillOfMaterialPriceHistories = new HashSet<BillOfMaterialPriceHistory>();
      this.EmployeeComplains = new HashSet<EmployeeComplain>();
      this.QAReviewCreatorUserEmployeeComplains = new HashSet<QAReviewEmployeeComplain>();
      this.QAReviewResponsibleUserEmployeeComplains = new HashSet<QAReviewEmployeeComplain>();
      this.PlanCodes = new HashSet<PlanCode>();
      this.StoreReceiptDeleteRequests = new HashSet<StoreReceiptDeleteRequest>();
      this.StoreReceiptDeleteRequestConfirmationLogs = new HashSet<StoreReceiptDeleteRequestConfirmationLog>();
      this.MeetingParticipants = new HashSet<MeetingParticipant>();
      this.CreatorOfTicketSoftwares = new HashSet<TicketSoftware>();
      this.ModifierOfTicketSoftwares = new HashSet<TicketSoftware>();
      this.TicketFile = new HashSet<TicketFile>();
      this.TicketComments = new HashSet<TicketComment>();
      this.StuffQualityControlObservations = new HashSet<StuffQualityControlObservation>();
      this.PriceAnnunciations = new HashSet<PriceAnnunciation>();
      this.PriceAnnunciationItems = new HashSet<PriceAnnunciationItem>();
      this.ExitReceiptDeleteRequestConfirmationLogs = new HashSet<ExitReceiptDeleteRequestConfirmationLog>();
      this.UserAssets = new HashSet<Asset>();
      this.UserAssetLogs = new HashSet<AssetLog>();
      this.RequestingUserAssetTransfers = new HashSet<AssetTransferRequest>();
      this.ConfirmerUserAssetTransfers = new HashSet<AssetTransferRequest>();
      this.StuffDocuments = new HashSet<StuffDocument>();
      this.ProjectERPs = new HashSet<ProjectERP>();
      this.ProjectERPEventRecords = new HashSet<ProjectERPEvent>();
      this.ProjectERPEventDocuments = new HashSet<ProjectERPEventDocument>();
      this.ProjectERPDocuments = new HashSet<ProjectERPDocument>();
      this.ProjectERPTasks = new HashSet<ProjectERPTask>();
      this.ProjectERPTaskDocuments = new HashSet<ProjectERPTaskDocument>();
      this.CargoItemLogs = new HashSet<CargoItemLog>();
      this.PriceInquries = new HashSet<PriceInquiry>();
      this.PaymentSuggestStatusLogs = new HashSet<PaymentSuggestStatusLog>();
    }
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public bool IsDelete { get; set; }
    public DateTime? DeletedAt { get; set; }
    public bool IsActive { get; set; }
    public bool IsLocked { get; set; }
    public DateTime LockOutDateTime { get; set; }
    public int LoginFailedCount { get; set; }
    public byte[] RowVersion { get; set; }
    public DateTime PasswordExpirationDate { get; set; }
    public bool HasAccessFromInternet { get; set; }
    public virtual ICollection<StuffPriceDiscrepancy> StuffPriceDiscrepancies { get; set; }
    public virtual ICollection<Membership> Memberships { get; set; }
    public virtual ICollection<Permission> Permissions { get; set; }
    public virtual Employee Employee { get; set; }
    public virtual ICollection<ScrumEntityComment> ScrumEntityComments { get; set; }
    public virtual ICollection<ScrumTask> ScrumTasks { get; set; }
    public virtual ICollection<ScrumEntityLog> ScrumEntityLogs { get; set; }
    public virtual ICollection<Message> Messages { get; set; }
    public virtual ICollection<MessageSend> MessageSends { get; set; }
    public virtual ICollection<UserPost> UserPosts { get; set; }
    public virtual ICollection<UserMessageRelation> ToUserMessageRelations { get; set; }
    public virtual ICollection<UserMessageRelation> FromUserMessageRelations { get; set; }
    public virtual ICollection<StockCheckingPerson> StockCheckingPersons { get; set; }
    public virtual ICollection<TagCounting> TagCountings { get; set; }
    public virtual ICollection<StockChecking> StockCheckings { get; set; }
    public virtual ICollection<TransactionBatch> TransactionBatches { get; set; }
    public virtual ICollection<BaseEntity> BaseEntities { get; set; }
    public virtual ICollection<ReportVersion> ReportVersions { get; set; }
    public virtual ICollection<Printer> Printers { get; set; }
    public virtual ICollection<ReportPrintSetting> PrinterSettings { get; set; }
    public virtual ICollection<Notification> Notifications { get; set; }
    public virtual ICollection<UserSetting> UserSettings { get; set; }
    public virtual ICollection<BaseEntityConfirmType> BaseEntityConfirmTypes { get; set; }
    public virtual ICollection<BaseEntityConfirmation> BaseEntityConfirmations { get; set; }
    public virtual ICollection<SuppliesPurchaserUser> SuppliesPurchaserUsers { get; set; }
    public virtual ICollection<FinancialLimit> FinancialLimits { get; set; }
    public virtual ICollection<QualityControl> QualityControls { get; set; }
    public virtual ICollection<BaseEntityDocument> BaseEntityDocuments { get; set; }
    public virtual ICollection<BaseEntityLog> BaseEntityLogs { get; set; }
    public virtual ICollection<FinancialTransactionBatch> FinancialTransactionBatches { get; set; }
    public virtual ICollection<BankOrderLog> BankOrderStateLogs { get; set; }
    public virtual ICollection<BillOfMaterial> BillOfMaterials { get; set; }
    public virtual ICollection<SendPermission> SendPermissions { get; set; }
    public virtual ICollection<SerialProfile> SerialProfiles { get; set; }
    public virtual ICollection<ConditionalQualityControl> ConditionalQualityControls { get; set; }
    public virtual ICollection<StuffPrice> StuffPrices { get; set; }
    public virtual ICollection<ProductionLineRepairUnit> ProductionLineRepairUnits { get; set; }
    public virtual ICollection<LadingCustomhouseLog> LadingCustomhouseLogs { get; set; }
    public virtual ICollection<StuffSerial> StuffSerials { get; set; }
    public virtual ICollection<BillOfMaterialDocument> BillOfMaterialDocuments { get; set; }
    public virtual ICollection<PurchaseOrderStepDetail> PurchaseOrderStepDetails { get; set; }
    public virtual ICollection<PurchaseRequestStepDetail> PurchaseRequestStepDetails { get; set; }
    public virtual ICollection<PurchaseOrderStep> PurchaseOrderSteps { get; set; }
    public virtual ICollection<PurchaseRequestStep> PurchaseRequestSteps { get; set; }
    public virtual ICollection<LadingBankOrderLog> LadingBankOrderLogs { get; set; }
    public virtual ICollection<UserToken> UserTokens { get; set; }
    public virtual ICollection<OrderDocument> OrderDocuments { get; set; }
    public virtual ICollection<LadingChangeRequest> DemandantLadingChangeRequests { get; set; }
    public virtual ICollection<LadingChangeRequest> ConfirmerLadingChangeRequests { get; set; }
    public virtual ICollection<ProvisionersCart> ProvisionersCarts { get; set; }
    public virtual ICollection<Allocation> Allocations { get; set; }
    public virtual ICollection<EnactmentActionProcessLog> EnactmentActionProcessLogs { get; set; }
    public virtual ICollection<EntityLog> EntityLogs { get; set; }
    public virtual ICollection<StuffDefinitionRequest> StuffDefinitionRequestRequesters { get; set; }
    public virtual ICollection<StuffDefinitionRequest> StuffDefinitionRequestConfirmers { get; set; }
    public virtual ICollection<FinanceConfirmation> FinanceConfirmations { get; set; }
    public virtual ICollection<FinanceItem> FinanceItems { get; set; }
    public virtual ICollection<FinanceItem> ReceivedFinanceItems { get; set; }
    public virtual ICollection<Finance> Finances { get; set; }
    public virtual ICollection<FinanceItemConfirmation> FinanceItemConfirmations { get; set; }
    public virtual ICollection<DetailedCodeConfirmationRequest> DetailedCodeConfirmationRequest { get; set; }
    public virtual ICollection<DetailedCodeConfirmationRequest> ConfirmerDetailedCodeConfirmationRequest { get; set; }
    public virtual ICollection<PurchaseRequestEditLog> PurchaseRequestEditLogs { get; set; }
    public virtual ICollection<OrganizationPost> OrganizationPosts { get; set; }
    public virtual ICollection<OrganizationJob> OrganizationJobs { get; set; }
    public virtual ICollection<OrganizationPostHistory> OrganizationPostHistories { get; set; }
    public virtual ICollection<FinanceAllocation> FinanceAllocations { get; set; }
    public virtual ICollection<Risk> Risks { get; set; }
    public virtual ICollection<RiskResolve> RiskResolveCreator { get; set; }
    public virtual ICollection<RiskResolve> RiskResolveReviewer { get; set; }
    public virtual ICollection<RiskStatus> RiskStatuses { get; set; }
    public virtual ICollection<EmployeeWorkReport> EmployeeWorkReports { get; set; }
    public virtual ICollection<CustomerComplaint> CustomerComplaints { get; set; }
    public virtual ICollection<CustomerComplaint> CorrectiveActions { get; set; }
    public virtual ICollection<EmployeeEvaluation> EmployeeEvaluations { get; set; }
    public virtual ICollection<EmployeeEvaluationItem> EmployeeEvaluationItems { get; set; }
    public virtual ICollection<EmployeeEvaluationPeriod> EmployeeEvaluationPeriods { get; set; }
    public virtual ICollection<QualityControlConfirmationTest> QualityControlConfirmationTests { get; set; }
    public virtual ICollection<QualityControlConfirmationTestItem> QualityControlConfirmationTestItems { get; set; }
    public virtual ICollection<SerialFailedOperation> ReviewerSerialFailedOperations { get; set; }
    public virtual ICollection<SerialFailedOperation> ConfirmSerialFailedOperations { get; set; }
    public virtual ICollection<ProductionOperatorEmployeeBan> ProductionOperatorEmployeeBans { get; set; }
    public virtual ICollection<LinkSerial> LinkSerials { get; set; }
    public virtual ICollection<LinkSerial> LinkerLinkSerials { get; set; }
    public virtual ICollection<GeneralStuffRequest> GeneralStuffRequests { get; set; }
    public virtual ICollection<CustomerComplaintSummary> CustomerComplaintSummaries { get; set; }
    public virtual ICollection<Proposal> ProposalRecommenderUsers { get; set; }
    public virtual ICollection<Proposal> Proposals { get; set; }
    public virtual ICollection<ProposalType> ProposalTypes { get; set; }
    public virtual ICollection<ProposalQAReview> ProposalQAReviews { get; set; }
    public virtual ICollection<ProposalReviewCommittee> ProposalReviewCommittees { get; set; }
    public virtual ICollection<ProposalQAReview> ProposalResponsibles { get; set; }
    public virtual ICollection<ProposalReviewCommittee> ProposalReviewCommitteeResponsibles { get; set; }
    public virtual ICollection<TestCondition> TestConditions { get; set; }
    public virtual ICollection<DepartmentManager> DepartmentManagers { get; set; }
    public virtual ICollection<ReturnSerialToPreviousStateRequest> DemandantSerialReturnRequests { get; set; }
    public virtual ICollection<ReturnSerialToPreviousStateRequest> ConfirmerSerilReturnRequests { get; set; }
    public virtual ICollection<ReturnSerialToPreviousStateRequest> WrongDoerSerilReturnRequests { get; set; }
    public virtual ICollection<CostCenter> ConfirmerCostCenters { get; set; }
    public virtual ICollection<PermissionRequest> PermissionRequestRegisterars { get; set; }
    public virtual ICollection<PermissionRequestAction> PermissionRequestConfirmators { get; set; }
    public virtual ICollection<PermissionRequest> PermissionRequestIntenders { get; set; }
    public virtual ICollection<IranKhodroSerial> IranKhodroSerials { get; set; }
    public virtual ICollection<QualityControlSample> QualityControlSamples { get; set; }
    public virtual ICollection<QualityControlSample> StatusChangerQualityControlSample { get; set; }
    public virtual ICollection<StuffFractionTemporaryStuff> StuffFractionReportTemporaryStuffs { get; set; }
    public virtual ICollection<MinutesMeeting> RegistrantMinutesMeetings { get; set; }
    public virtual ICollection<MinutesMeeting> SecretaryMinutesMeetings { get; set; }
    public virtual ICollection<MinutesMeeting> BossMinutesMeetings { get; set; }
    public virtual ICollection<MeetingApproval> OperatorMeetingAprovals { get; set; }
    public virtual ICollection<BillOfMaterialPriceHistory> BillOfMaterialPriceHistories { get; set; }
    public virtual ICollection<EmployeeComplain> EmployeeComplains { get; set; }
    public virtual ICollection<QAReviewEmployeeComplain> QAReviewCreatorUserEmployeeComplains { get; set; }
    public virtual ICollection<QAReviewEmployeeComplain> QAReviewResponsibleUserEmployeeComplains { get; set; }
    public virtual ICollection<PlanCode> PlanCodes { get; set; }
    public virtual ICollection<StoreReceiptDeleteRequest> StoreReceiptDeleteRequests { get; set; }
    public virtual ICollection<StoreReceiptDeleteRequestConfirmationLog> StoreReceiptDeleteRequestConfirmationLogs { get; set; }
    public virtual ICollection<MeetingParticipant> MeetingParticipants { get; set; }
    public virtual ICollection<TicketSoftware> CreatorOfTicketSoftwares { get; set; }
    public virtual ICollection<TicketSoftware> ModifierOfTicketSoftwares { get; set; }
    public virtual ICollection<TicketComment> TicketComments { get; set; }
    public virtual ICollection<TicketFile> TicketFile { get; set; }
    public virtual ICollection<StuffQualityControlObservation> StuffQualityControlObservations { get; set; }
    public virtual ICollection<PriceAnnunciation> PriceAnnunciations { get; set; }
    public virtual ICollection<PriceAnnunciationItem> PriceAnnunciationItems { get; set; }
    public virtual ICollection<ExitReceiptDeleteRequestConfirmationLog> ExitReceiptDeleteRequestConfirmationLogs { get; set; }
    public virtual ICollection<Asset> UserAssets { get; set; }
    public virtual ICollection<AssetLog> UserAssetLogs { get; set; }
    public virtual ICollection<AssetTransferRequest> RequestingUserAssetTransfers { get; set; }
    public virtual ICollection<AssetTransferRequest> ConfirmerUserAssetTransfers { get; set; }
    public virtual ICollection<StuffDocument> StuffDocuments { get; set; }
    public virtual ICollection<ProjectERP> ProjectERPs { get; set; }
    public virtual ICollection<ProjectERPEvent> ProjectERPEventRecords { get; set; }
    public virtual ICollection<ProjectERPEventDocument> ProjectERPEventDocuments { get; set; }
    public virtual ICollection<ProjectERPDocument> ProjectERPDocuments { get; set; }
    public virtual ICollection<ProjectERPTask> ProjectERPTasks { get; set; }
    public virtual ICollection<ProjectERPTaskDocument> ProjectERPTaskDocuments { get; set; }
    public virtual ICollection<CargoItemLog> CargoItemLogs { get; set; }
    public virtual ICollection<PriceInquiry> PriceInquries { get; set; }
    public virtual ICollection<PaymentSuggestStatusLog> PaymentSuggestStatusLogs { get; set; }
  }
}