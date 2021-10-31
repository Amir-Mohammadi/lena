using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class Employee : IEntity
  {
    protected internal Employee()
    {
      this.ProductionOperationEmployees = new HashSet<ProductionOperationEmployee>();
      this.ProductionOperatorEmployees = new HashSet<ProductionOperatorMachineEmployee>();
      this.Stuffs = new HashSet<Stuff>();
      this.QualityControls = new HashSet<QualityControl>();
      this.EmployeeOperatorTypes = new HashSet<EmployeeOperatorType>();
      this.WarehouseIssues = new HashSet<WarehouseIssue>();
      this.StuffRequests = new HashSet<StuffRequest>();
      this.ProductionLineEmployeeIntervals = new HashSet<ProductionLineEmployeeInterval>();
      this.ProductionOrders = new HashSet<ProductionOrder>();
      this.OrganizationPostHistories = new HashSet<OrganizationPostHistory>();
      this.EmployeeWorkReports = new HashSet<EmployeeWorkReport>();
      this.EmployeeEvaluations = new HashSet<EmployeeEvaluation>();
      this.ProductionOperatorEmployeeBans = new HashSet<ProductionOperatorEmployeeBan>();
      this.GeneralStuffRequests = new HashSet<GeneralStuffRequest>();
      this.SoftwareWorkReports = new HashSet<SoftwareWorkReport>();
      this.EmployeeComplains = new HashSet<EmployeeComplain>();
      this.MeetingParticipants = new HashSet<MeetingParticipant>();
      this.PurchaseRequestResponsibleEmployees = new HashSet<PurchaseRequest>();
      this.PurchaseRequestEmployeeRequesters = new HashSet<PurchaseRequest>();
      this.EmployeeAssets = new HashSet<Asset>();
      this.EmployeeAssetLogs = new HashSet<AssetLog>();
      this.NewEmployeeAssetTransferRequests = new HashSet<AssetTransferRequest>();
      this.ProjectERPResponsibleEmployees = new HashSet<ProjectERPResponsibleEmployee>();
      this.ProjectERPTasks = new HashSet<ProjectERPTask>();
      this.ProjectERPEvents = new HashSet<ProjectERPEvent>();
      this.Contacts = new HashSet<Contact>();
    }
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Nullable<DateTime> EmployeementDate { get; set; }
    public byte[] Image { get; set; }
    public string Code { get; set; }
    public string NationalCode { get; set; }
    public string FatherName { get; set; }
    public Nullable<DateTime> BirthDate { get; set; }
    public string BirthPlace { get; set; }
    public bool IsActive { get; set; }
    public Nullable<Guid> DocumentId { get; set; }
    public Nullable<short> DepartmentId { get; set; }
    public byte[] RowVersion { get; set; }
    public int? OrgnizationPostId { get; set; }
    public int? UserId { get; set; }
    public virtual OrganizationPost OrganizationPost { get; set; }
    public int? OrganizationJobId { get; set; }
    public virtual OrganizationJob OrganizationJob { get; set; }
    public virtual User User { get; set; }
    public virtual Department Department { get; set; }
    public virtual ICollection<ProductionOperationEmployee> ProductionOperationEmployees { get; set; }
    public virtual ICollection<ProductionOperatorMachineEmployee> ProductionOperatorEmployees { get; set; }
    public virtual ICollection<Stuff> Stuffs { get; set; }
    public virtual ICollection<QualityControl> QualityControls { get; set; }
    public virtual ICollection<EmployeeOperatorType> EmployeeOperatorTypes { get; set; }
    public virtual Supplier Supplier { get; set; }
    public virtual ICollection<WarehouseIssue> WarehouseIssues { get; set; }
    public virtual ICollection<StuffRequest> StuffRequests { get; set; }
    public virtual ProductionTerminal ProductionTerminal { get; set; }
    public virtual ICollection<ProductionLineEmployeeInterval> ProductionLineEmployeeIntervals { get; set; }
    public virtual ICollection<ProductionOrder> ProductionOrders { get; set; }
    public virtual ICollection<OrganizationPostHistory> OrganizationPostHistories { get; set; }
    public virtual ICollection<EmployeeWorkReport> EmployeeWorkReports { get; set; }
    public virtual ICollection<EmployeeEvaluation> EmployeeEvaluations { get; set; }
    public virtual ICollection<ProductionOperatorEmployeeBan> ProductionOperatorEmployeeBans { get; set; }
    public virtual ICollection<GeneralStuffRequest> GeneralStuffRequests { get; set; }
    public virtual ICollection<SoftwareWorkReport> SoftwareWorkReports { get; set; }
    public virtual ICollection<Asset> EmployeeAssets { get; set; }
    public virtual ICollection<AssetLog> EmployeeAssetLogs { get; set; }
    public virtual ICollection<AssetTransferRequest> NewEmployeeAssetTransferRequests { get; set; }
    public virtual ICollection<EmployeeComplain> EmployeeComplains { get; set; }
    public virtual ICollection<MeetingParticipant> MeetingParticipants { get; set; }
    public virtual ICollection<PurchaseRequest> PurchaseRequestResponsibleEmployees { get; set; }
    public virtual ICollection<PurchaseRequest> PurchaseRequestEmployeeRequesters { get; set; }
    public virtual ICollection<ProjectERPResponsibleEmployee> ProjectERPResponsibleEmployees { get; set; }
    public virtual ICollection<ProjectERPTask> ProjectERPTasks { get; set; }
    public virtual ICollection<ProjectERPEvent> ProjectERPEvents { get; set; }
    public virtual ICollection<Contact> Contacts { get; set; }
  }
}