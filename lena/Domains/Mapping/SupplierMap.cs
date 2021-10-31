using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class SupplierMap : IEntityTypeConfiguration<Supplier>
  {
    public void Configure(EntityTypeBuilder<Supplier> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("Suppliers");
      builder.Property(x => x.Id);
      builder.Property(x => x.IsActive);
      builder.Property(x => x.EmployeeId);
      builder.Property(x => x.Description);
      builder.HasRowVersion();
    }
  }
}