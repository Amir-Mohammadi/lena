using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ResponsibleDepartmentMap : IEntityTypeConfiguration<ResponsibleDepartment>
  {
    public void Configure(EntityTypeBuilder<ResponsibleDepartment> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("ResponsibleDepartment");
      builder.Property(x => x.UserId);
      builder.Property(x => x.EmployeeComplainDepartmentId);
      builder.Property(x => x.Opinion);
      builder.HasRowVersion();
      builder.HasOne(x => x.EmployeeComplainDepartment).WithMany(x => x.ResponsibleDepartments).HasForeignKey(x => x.EmployeeComplainDepartmentId);
    }
  }
}
