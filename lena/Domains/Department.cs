using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class Department : IEntity
  {
    protected internal Department()
    {
      this.ScrumEntities = new HashSet<ScrumEntity>();
      this.ChildDepartments = new HashSet<Department>();
      this.Employees = new HashSet<Employee>();
      this.DepartmentWorkShifts = new HashSet<DepartmentWorkShift>();
      this.Stuffs = new HashSet<Stuff>();
      this.QualityControls = new HashSet<QualityControl>();
      this.ProductionLines = new HashSet<ProductionLine>();
      this.Warehouses = new HashSet<Warehouse>();
      this.BaseEntityConfirmTypes = new HashSet<BaseEntityConfirmType>();
      this.WarehouseIssues = new HashSet<WarehouseIssue>();
      this.StuffRequests = new HashSet<StuffRequest>();
      this.Indicators = new HashSet<Indicator>();
      this.StuffPurchaseCategories = new HashSet<StuffPurchaseCategory>();
      this.ProductionPerformanceInfoes = new HashSet<ProductionPerformanceInfo>();
      this.CustomerComplaintDepartments = new HashSet<CustomerComplaintDepartment>();
      this.GeneralStuffRequests = new HashSet<GeneralStuffRequest>();
      this.CustomerComplaintDepartments = new HashSet<CustomerComplaintDepartment>();
      this.EvaluationCategories = new HashSet<EvaluationCategory>();
      this.EmployeeComplainDepartments = new HashSet<EmployeeComplainDepartment>();
      this.PurchaseRequests = new HashSet<PurchaseRequest>();
      this.DepartmentAssets = new HashSet<Asset>();
      this.DepartmentAssetLogs = new HashSet<AssetLog>();
      this.NewDepartmentAssetTransferRequests = new HashSet<AssetTransferRequest>();
      this.MeetingApprovals = new HashSet<MeetingApproval>();
      this.IndicatorWeights = new HashSet<IndicatorWeight>();
    }
    public short Id { get; set; }
    public string Name { get; set; }
    public Nullable<short> ParentDepartmentId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<PurchaseRequest> PurchaseRequests { get; set; }
    public virtual ICollection<ScrumEntity> ScrumEntities { get; set; }
    public virtual ICollection<Department> ChildDepartments { get; set; }
    public virtual Department ParentDepartment { get; set; }
    public virtual ICollection<Employee> Employees { get; set; }
    public virtual ICollection<DepartmentWorkShift> DepartmentWorkShifts { get; set; }
    public virtual ICollection<Stuff> Stuffs { get; set; }
    public virtual ICollection<QualityControl> QualityControls { get; set; }
    public virtual ICollection<ProductionLine> ProductionLines { get; set; }
    public virtual ICollection<Warehouse> Warehouses { get; set; }
    public virtual ICollection<BaseEntityConfirmType> BaseEntityConfirmTypes { get; set; }
    public virtual ICollection<WarehouseIssue> WarehouseIssues { get; set; }
    public virtual ICollection<StuffRequest> StuffRequests { get; set; }
    public virtual ICollection<Indicator> Indicators { get; set; }
    public virtual ICollection<StuffPurchaseCategory> StuffPurchaseCategories { get; set; }
    public virtual ICollection<ProductionPerformanceInfo> ProductionPerformanceInfoes { get; set; }
    public virtual ICollection<CustomerComplaintDepartment> CustomerComplaintDepartments { get; set; }
    public virtual ICollection<GeneralStuffRequest> GeneralStuffRequests { get; set; }
    public virtual ICollection<EvaluationCategory> EvaluationCategories { get; set; }
    public virtual ICollection<EmployeeComplainDepartment> EmployeeComplainDepartments { get; set; }
    public virtual ICollection<MeetingApproval> MeetingApprovals { get; set; }
    public virtual DepartmentManager DepartmentManager { get; set; }
    public virtual ICollection<Asset> DepartmentAssets { get; set; }
    public virtual ICollection<AssetLog> DepartmentAssetLogs { get; set; }
    public virtual ICollection<AssetTransferRequest> NewDepartmentAssetTransferRequests { get; set; }
    public virtual ICollection<IndicatorWeight> IndicatorWeights { get; set; }
  }
}