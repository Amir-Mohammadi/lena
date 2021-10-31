using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class QualityControlMap : IEntityTypeConfiguration<QualityControl>
  {
    public void Configure(EntityTypeBuilder<QualityControl> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_QualityControl");
      builder.Property(x => x.Id);
      builder.Property(x => x.QualityControlType);
      builder.Property(x => x.StuffId);
      builder.Property(x => x.WarehouseId);
      builder.Property(x => x.Status);
      builder.Property(x => x.UnitId);
      builder.Property(x => x.Qty);
      builder.Property(x => x.DepartmentId);
      builder.Property(x => x.EmployeeId);
      builder.Property(x => x.DocumentId);
      builder.Property(x => x.ConfirmationDateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.ConfirmationUserId);
      builder.Property(x => x.QualityControlPaymentSuggestStatus);
      builder.HasOne(x => x.Stuff).WithMany(x => x.QualityControls).HasForeignKey(x => x.StuffId);
      builder.HasOne(x => x.Warehouse).WithMany(x => x.QualityControls).HasForeignKey(x => x.WarehouseId);
      builder.HasOne(x => x.QualityControlConfirmation).WithOne(x => x.QualityControl).HasForeignKey<QualityControlConfirmation>(x => x.QualityControlId);
      builder.HasOne(x => x.Unit).WithMany(x => x.QualityControls).HasForeignKey(x => x.UnitId);
      builder.HasOne(x => x.Department).WithMany(x => x.QualityControls).HasForeignKey(x => x.DepartmentId);
      builder.HasOne(x => x.Employee).WithMany(x => x.QualityControls).HasForeignKey(x => x.EmployeeId);
      builder.HasOne(x => x.ConfirmationUser).WithMany(x => x.QualityControls).HasForeignKey(x => x.ConfirmationUserId);
    }
  }
}