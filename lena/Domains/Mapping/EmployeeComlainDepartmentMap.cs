using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class EmployeeComplainDepartmentMap : IEntityTypeConfiguration<EmployeeComplainDepartment>
  {
    public void Configure(EntityTypeBuilder<EmployeeComplainDepartment> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("EmployeeComplainDepartment");
      builder.Property(x => x.DepartmentId);
      builder.Property(x => x.EmployeeComplainItemId);
      builder.HasRowVersion();
      builder.HasOne(x => x.Department).WithMany(x => x.EmployeeComplainDepartments).HasForeignKey(x => x.DepartmentId);
      builder.HasOne(x => x.EmployeeComplainItem).WithMany(x => x.EmployeeComplainDepartments).HasForeignKey(x => x.EmployeeComplainItemId);
    }
  }
}
