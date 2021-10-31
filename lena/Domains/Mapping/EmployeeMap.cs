using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class EmployeeMap : IEntityTypeConfiguration<Employee>
  {
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("Employees");
      builder.Property(x => x.Id);
      builder.Property(x => x.FirstName).IsRequired();
      builder.Property(x => x.LastName).IsRequired();
      builder.Property(x => x.EmployeementDate).HasColumnType("smalldatetime");
      builder.Property(x => x.Image);
      builder.Property(x => x.Code).IsRequired().HasMaxLength(50);
      builder.Property(x => x.NationalCode);
      builder.Property(x => x.FatherName);
      builder.Property(x => x.BirthDate).HasColumnType("smalldatetime");
      builder.Property(x => x.BirthPlace);
      builder.Property(x => x.IsActive);
      builder.Property(x => x.DepartmentId);
      builder.Property(x => x.DocumentId);
      builder.Property(x => x.UserId);
      builder.HasRowVersion();
      builder.HasOne(x => x.User).WithOne(x => x.Employee).HasForeignKey<Employee>(x => x.UserId);
      builder.HasOne(x => x.Department).WithMany(x => x.Employees).HasForeignKey(x => x.DepartmentId);
      builder.HasOne(x => x.Supplier).WithOne(x => x.Employee).HasForeignKey<Supplier>(x => x.EmployeeId);
      builder.HasOne(x => x.OrganizationPost).WithMany(x => x.Employees).HasForeignKey(x => x.OrgnizationPostId);
      builder.HasOne(x => x.OrganizationJob).WithMany(x => x.Employees).HasForeignKey(x => x.OrganizationJobId);
    }
  }
}