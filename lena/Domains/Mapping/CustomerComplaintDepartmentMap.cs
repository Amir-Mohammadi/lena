using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class CustomerComplaintDepartmentMap : IEntityTypeConfiguration<CustomerComplaintDepartment>
  {
    public void Configure(EntityTypeBuilder<CustomerComplaintDepartment> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("CustomerComplaintDepartments");
      builder.Property(x => x.Id);
      builder.HasRowVersion();
      builder.Property(x => x.CustomerComplaintSummaryId);
      builder.Property(x => x.InhibitionAction);
      builder.Property(x => x.DepartmentId);
      builder.Property(x => x.DateOfInhibition);
      builder.HasOne(x => x.Department).WithMany(x => x.CustomerComplaintDepartments).HasForeignKey(x => x.DepartmentId);
      builder.HasOne(x => x.CustomerComplaintSummary).WithMany(x => x.CustomerComplaintDepartments).HasForeignKey(x => x.CustomerComplaintSummaryId);
    }
  }
}