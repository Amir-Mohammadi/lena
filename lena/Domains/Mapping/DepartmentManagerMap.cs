using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class DepartmentManagerMap : IEntityTypeConfiguration<DepartmentManager>
  {
    public void Configure(EntityTypeBuilder<DepartmentManager> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("DepartmentManager");
      builder.Property(x => x.DateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.UserId);
      builder.Property(x => x.OrganizationPostId);
      builder.Property(x => x.DepartmentId);
      builder.HasRowVersion();
      builder.HasOne(x => x.User).WithMany(x => x.DepartmentManagers).HasForeignKey(x => x.UserId);
      builder.HasOne(x => x.OrganizationPost).WithOne(x => x.DepartmentManager).HasForeignKey<DepartmentManager>(x => x.OrganizationPostId);
      builder.HasOne(x => x.Department).WithOne(x => x.DepartmentManager).HasForeignKey<DepartmentManager>(x => x.DepartmentId);
    }
  }
}