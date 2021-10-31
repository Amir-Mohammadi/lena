using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class DepartmentMap : IEntityTypeConfiguration<Department>
  {
    public void Configure(EntityTypeBuilder<Department> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("Departments");
      builder.Property(x => x.Id);
      builder.Property(x => x.Name).IsRequired();
      builder.Property(x => x.ParentDepartmentId);
      builder.HasRowVersion();
      builder.HasOne(x => x.ParentDepartment).WithMany(x => x.ChildDepartments).HasForeignKey(x => x.ParentDepartmentId);
    }
  }
}
