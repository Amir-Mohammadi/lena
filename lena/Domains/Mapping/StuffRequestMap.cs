using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class StuffRequestMap : IEntityTypeConfiguration<StuffRequest>
  {
    public void Configure(EntityTypeBuilder<StuffRequest> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_StuffRequest");
      builder.Property(x => x.Id);
      builder.Property(x => x.StuffRequestType);
      builder.Property(x => x.ScrumEntityId);
      builder.Property(x => x.FromWarehouseId);
      builder.Property(x => x.ToWarehouseId);
      builder.Property(x => x.ProductionMaterialRequestId);
      builder.Property(x => x.ToEmployeeId);
      builder.Property(x => x.ToDepartmentId);
      builder.HasOne(x => x.ScrumEntity).WithMany(x => x.StuffRequests).HasForeignKey(x => x.ScrumEntityId);
      builder.HasOne(x => x.FromWarehouse).WithMany(x => x.ExportStuffRequests).HasForeignKey(x => x.FromWarehouseId);
      builder.HasOne(x => x.ToWarehouse).WithMany(x => x.ImportStuffRequests).HasForeignKey(x => x.ToWarehouseId);
      builder.HasOne(x => x.ProductionMaterialRequest).WithMany(x => x.StuffRequests).HasForeignKey(x => x.ProductionMaterialRequestId);
      builder.HasOne(x => x.ToEmployee).WithMany(x => x.StuffRequests).HasForeignKey(x => x.ToEmployeeId);
      builder.HasOne(x => x.ToDepartment).WithMany(x => x.StuffRequests).HasForeignKey(x => x.ToDepartmentId);
    }
  }
}
