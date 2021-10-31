using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class GeneralStuffRequestMap : IEntityTypeConfiguration<GeneralStuffRequest>
  {
    public void Configure(EntityTypeBuilder<GeneralStuffRequest> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("GeneralStuffRequests");
      builder.Property(x => x.Id);
      builder.Property(x => x.StuffRequestType);
      builder.Property(x => x.ScrumEntityId);
      builder.Property(x => x.FromWarehouseId);
      builder.Property(x => x.ToWarehouseId);
      builder.Property(x => x.ProductionMaterialRequestId);
      builder.Property(x => x.ToEmployeeId);
      builder.Property(x => x.ToDepartmentId);
      builder.Property(x => x.StatusDescription).IsRequired(false);
      builder.Property(x => x.Deadline);
      builder.Property(x => x.StuffRequestQty);
      builder.Property(x => x.PurchaseRequestQty);
      builder.Property(x => x.RowVersion).IsRowVersion().IsRequired();
      builder.HasOne(x => x.FromWarehosue).WithMany(x => x.ExportGeneralStuffRequests).HasForeignKey(x => x.FromWarehouseId);
      builder.HasOne(x => x.ToWarehouse).WithMany(x => x.ImportGeneralStuffRequests).HasForeignKey(x => x.ToWarehouseId);
      builder.HasOne(x => x.ProductionMaterialRequest).WithMany(x => x.GeneralStuffRequests).HasForeignKey(x => x.ProductionMaterialRequestId);
      builder.HasOne(x => x.ToEmployee).WithMany(x => x.GeneralStuffRequests).HasForeignKey(x => x.ToEmployeeId);
      builder.HasOne(x => x.ToDepartment).WithMany(x => x.GeneralStuffRequests).HasForeignKey(x => x.ToDepartmentId);
      builder.HasOne(x => x.Stuff).WithMany(x => x.GeneralStuffRequests).HasForeignKey(x => x.StuffId);
      builder.HasOne(x => x.Unit).WithMany(x => x.GeneralStuffRequests).HasForeignKey(x => x.UnitId);
      builder.HasOne(x => x.ScrumEntity).WithMany(x => x.GeneralStuffRequests).HasForeignKey(x => x.ScrumEntityId);
      builder.HasOne(x => x.BillOfMaterial).WithMany(x => x.GeneralStuffRequests).HasForeignKey(x => new
      {
        x.BillOfMaterialVersion,
        x.StuffId
      });
    }
  }
}