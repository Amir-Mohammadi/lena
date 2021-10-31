using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class PurchaseRequestMap : IEntityTypeConfiguration<PurchaseRequest>
  {
    public void Configure(EntityTypeBuilder<PurchaseRequest> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_PurchaseRequest");
      builder.Property(x => x.Id);
      builder.Property(x => x.Deadline);
      builder.Property(x => x.Qty);
      builder.Property(x => x.RequestQty);
      builder.Property(x => x.UnitId);
      builder.Property(x => x.StuffId);
      builder.Property(x => x.Status);
      builder.Property(x => x.ResponsibleEmployeeId);
      builder.Property(x => x.OldPlanCode);
      builder.Property(x => x.ProjectCode);
      builder.Property(x => x.IsArchived);
      builder.Property(x => x.PurchaseRequestStepDetailId);
      builder.Property(x => x.CostCenterId);
      builder.Property(x => x.Essential);
      builder.Property(x => x.PlanCodeId);
      builder.Property(x => x.DocumentId);
      builder.Property(x => x.EmployeeRequesterId);
      builder.Property(x => x.Link);
      builder.Property(x => x.DepartmentId);
      builder.Property(x => x.LatestRiskId);
      builder.HasOne(x => x.Department).WithMany(x => x.PurchaseRequests).HasForeignKey(x => x.DepartmentId);
      builder.Property(x => x.SupplyType);
      builder.HasOne(x => x.Unit).WithMany(x => x.PurchaseRequests).HasForeignKey(x => x.UnitId);
      builder.HasOne(x => x.Stuff).WithMany(x => x.PurchaseRequests).HasForeignKey(x => x.StuffId);
      builder.HasOne(x => x.PurchaseRequestSummary).WithOne(x => x.PurchaseRequest).HasForeignKey<PurchaseRequestSummary>(x => x.PurchaseRequestId);
      builder.HasOne(x => x.PurchaseRequestStepDetail).WithMany(x => x.PurchaseRequests).HasForeignKey(x => x.PurchaseRequestStepDetailId);
      builder.HasOne(x => x.LatestRisk).WithOne().HasForeignKey<PurchaseRequest>(x => x.LatestRiskId);
      builder.HasOne(x => x.CostCenter).WithMany(x => x.PurchaseRequests).HasForeignKey(x => x.CostCenterId);
      builder.HasOne(x => x.PlanCode).WithMany(x => x.PurchaseRequests).HasForeignKey(x => x.PlanCodeId);
      builder.HasOne(x => x.ResponsibleEmployee).WithMany(x => x.PurchaseRequestResponsibleEmployees).HasForeignKey(x => x.ResponsibleEmployeeId);
      builder.HasOne(x => x.EmployeeRequester).WithMany(x => x.PurchaseRequestEmployeeRequesters).HasForeignKey(x => x.EmployeeRequesterId);
    }
  }
}