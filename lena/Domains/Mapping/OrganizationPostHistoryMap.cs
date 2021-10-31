using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class OrganizationPostHistoryMap : IEntityTypeConfiguration<OrganizationPostHistory>
  {
    public void Configure(EntityTypeBuilder<OrganizationPostHistory> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("OrganizationPostHistories");
      builder.Property(x => x.CreationTime).HasColumnType("smalldatetime");
      builder.Property(x => x.StartDate).HasColumnType("smalldatetime");
      builder.Property(x => x.Description);
      builder.HasRowVersion();
      builder.HasOne(x => x.Employee).WithMany(x => x.OrganizationPostHistories).HasForeignKey(x => x.EmployeeId);
      //builder.HasOne(x => x.OrganizationPost).WithMany(x => x.PostHistories).HasForeignKey(x => x.OrganizationPostId);
    }
  }
}