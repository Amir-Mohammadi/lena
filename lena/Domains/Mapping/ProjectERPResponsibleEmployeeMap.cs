using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ProjectERPResponsibleEmployeeMap : IEntityTypeConfiguration<ProjectERPResponsibleEmployee>
  {
    public void Configure(EntityTypeBuilder<ProjectERPResponsibleEmployee> builder)
    {
      builder.HasKey(x => new
      {
        x.ProjectERPId,
        x.ResponsibleEmployeeId
      });
      builder.ToTable("ProjectERPResponsibleEmployees");
      builder.Property(x => x.ProjectERPId);
      builder.Property(x => x.ResponsibleEmployeeId);
      builder.Property(x => x.CreatorDateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.IsActive);
      builder.Property(x => x.Description);
      builder.HasRowVersion();
      builder.HasOne(x => x.ResponsibleEmployee).WithMany(x => x.ProjectERPResponsibleEmployees).HasForeignKey(x => x.ResponsibleEmployeeId);
      builder.HasOne(x => x.ProjectERP).WithMany(x => x.ProjectERPResponsibleEmployees).HasForeignKey(x => x.ProjectERPId);
    }
  }
}
