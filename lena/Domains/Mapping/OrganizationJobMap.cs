using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class OrganizationJobMap : IEntityTypeConfiguration<OrganizationJob>
  {
    public void Configure(EntityTypeBuilder<OrganizationJob> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("OrganizationJobs");
      builder.Property(x => x.Title).IsRequired().HasMaxLength(255);
      builder.Property(x => x.Description).HasMaxLength(512);
      builder.Property(x => x.IsActive);
      builder.Property(x => x.CreationTime).HasColumnType("smalldatetime");
      builder.HasRowVersion();
      builder.HasOne(x => x.Creator).WithMany(x => x.OrganizationJobs).HasForeignKey(x => x.CreatorId);
      builder.HasOne(x => x.OrganizationPost).WithMany(x => x.OrganizationJobs).HasForeignKey(x => x.OranizationPostId);
    }
  }
}