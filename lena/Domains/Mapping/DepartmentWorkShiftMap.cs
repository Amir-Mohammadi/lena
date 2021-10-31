using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class DepartmentWorkShiftMap : IEntityTypeConfiguration<DepartmentWorkShift>
  {
    public void Configure(EntityTypeBuilder<DepartmentWorkShift> builder)
    {
      builder.HasKey(x => new
      {
        x.WorkShiftId,
        x.DepartmentId
      });
      builder.ToTable("DepartmentWorkShifts");
      builder.Property(x => x.WorkShiftId);
      builder.Property(x => x.DepartmentId);
      builder.HasRowVersion();
      builder.HasOne(x => x.Department).WithMany(x => x.DepartmentWorkShifts).HasForeignKey(x => x.DepartmentId);
      builder.HasOne(x => x.WorkShift).WithMany(x => x.DepartmentWorkShifts).HasForeignKey(x => x.WorkShiftId);
    }
  }
}
