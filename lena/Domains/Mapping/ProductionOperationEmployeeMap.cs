using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ProductionOperationEmployeeMap : IEntityTypeConfiguration<ProductionOperationEmployee>
  {
    public void Configure(EntityTypeBuilder<ProductionOperationEmployee> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("ProductionOperationEmployees");
      builder.Property(x => x.Id);
      builder.Property(x => x.EmployeeId);
      builder.Property(x => x.ProductionOperationEmployeeGroupId);
      builder.HasRowVersion();
      builder.HasOne(x => x.Employee).WithMany(x => x.ProductionOperationEmployees).HasForeignKey(x => x.EmployeeId);
      builder.HasOne(x => x.GetProductionOperationEmployeeGroup).WithMany(x => x.ProductionOperationEmployees).HasForeignKey(x => x.ProductionOperationEmployeeGroupId);
    }
  }
}
