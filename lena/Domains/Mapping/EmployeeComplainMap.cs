using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class EmployeeComplainMap : IEntityTypeConfiguration<EmployeeComplain>
  {
    public void Configure(EntityTypeBuilder<EmployeeComplain> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("EmployeeComplain");
      builder.Property(x => x.UserId);
      builder.Property(x => x.EmployeeId);
      builder.Property(x => x.DateTime).HasColumnType("smalldatetime");
      builder.HasRowVersion();
      builder.HasOne(x => x.User).WithMany(x => x.EmployeeComplains).HasForeignKey(x => x.UserId);
      builder.HasOne(x => x.Employee).WithMany(x => x.EmployeeComplains).HasForeignKey(x => x.EmployeeId);
    }
  }
}
