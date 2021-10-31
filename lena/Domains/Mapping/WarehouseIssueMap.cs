using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class WarehouseIssueMap : IEntityTypeConfiguration<WarehouseIssue>
  {
    public void Configure(EntityTypeBuilder<WarehouseIssue> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_WarehouseIssue");
      builder.Property(x => x.Id);
      builder.Property(x => x.Status);
      builder.Property(x => x.FromWarehouseId);
      builder.Property(x => x.ToWarehouseId);
      builder.Property(x => x.ToDepartmentId);
      builder.Property(x => x.ToEmployeeId);
      builder.HasOne(x => x.FromWarehouse).WithMany(x => x.ExportWarehouseIssues).HasForeignKey(x => x.FromWarehouseId);
      builder.HasOne(x => x.ToWarehouse).WithMany(x => x.ImportWarehouseIssues).HasForeignKey(x => x.ToWarehouseId);
      builder.HasOne(x => x.ToDepartment).WithMany(x => x.WarehouseIssues).HasForeignKey(x => x.ToDepartmentId);
      builder.HasOne(x => x.ToEmployee).WithMany(x => x.WarehouseIssues).HasForeignKey(x => x.ToEmployeeId);
    }
  }
}