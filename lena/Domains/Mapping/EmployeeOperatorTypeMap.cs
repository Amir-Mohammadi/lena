using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class EmployeeOperatorTypeMap : IEntityTypeConfiguration<EmployeeOperatorType>
  {
    public void Configure(EntityTypeBuilder<EmployeeOperatorType> builder)
    {
      builder.HasKey(x => new
      {
        x.EmployeeId,
        x.OperatorTypeId
      });
      builder.ToTable("EmployeeOperatorTypes");
      builder.Property(x => x.EmployeeId);
      builder.Property(x => x.OperatorTypeId);
      builder.HasRowVersion();
      builder.HasOne(x => x.Employee).WithMany(x => x.EmployeeOperatorTypes).HasForeignKey(x => x.EmployeeId);
      builder.HasOne(x => x.OperatorType).WithMany(x => x.EmployeeOperatorTypes).HasForeignKey(x => x.OperatorTypeId);
    }
  }
}