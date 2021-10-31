using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class EmployeeComplainItemMap : IEntityTypeConfiguration<EmployeeComplainItem>
  {
    public void Configure(EntityTypeBuilder<EmployeeComplainItem> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("EmployeeComplainItem");
      builder.Property(x => x.EmployeeComplainId);
      builder.Property(x => x.Type);
      builder.Property(x => x.Description);
      builder.HasRowVersion();
      builder.HasOne(x => x.EmployeeComplain).WithMany(x => x.EmployeeComplainItems).HasForeignKey(x => x.EmployeeComplainId);
    }
  }
}
