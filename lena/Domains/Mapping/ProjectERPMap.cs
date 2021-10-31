using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ProjectERPMap : IEntityTypeConfiguration<ProjectERP>
  {
    public void Configure(EntityTypeBuilder<ProjectERP> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("ProjectERPs");
      builder.Property(x => x.Title);
      builder.Property(x => x.Code);
      builder.Property(x => x.Version);
      builder.Property(x => x.IsActive);
      builder.Property(x => x.Priority);
      builder.Property(x => x.Progress);
      builder.Property(x => x.CreateDateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.StuffId).IsRequired(false);
      builder.Property(x => x.CustomerId).IsRequired(false);
      builder.Property(x => x.ProjectERPPhaseId).IsRequired(false);
      builder.Property(x => x.CreatorUserId);
      builder.Property(x => x.ProjectERPCategoryId);
      builder.Property(x => x.ProjectERPTypeId).IsRequired(false);
      builder.Property(x => x.EstimateStartDateTime).HasColumnType("smalldatetime").IsRequired(false).HasColumnType("smalldatetime");
      builder.Property(x => x.RealStartDateTime).HasColumnType("smalldatetime").IsRequired(false).HasColumnType("smalldatetime");
      builder.Property(x => x.Description);
      builder.Property(x => x.RowVersion).IsRowVersion().IsRequired();
      builder.HasOne(x => x.CreatorUser).WithMany(x => x.ProjectERPs).HasForeignKey(x => x.CreatorUserId);
      builder.HasOne(x => x.Stuff).WithMany(x => x.ProjectERPs).HasForeignKey(x => x.StuffId);
      builder.HasOne(x => x.Customer).WithMany(x => x.ProjectERPs).HasForeignKey(x => x.CustomerId);
      builder.HasOne(x => x.ProjectERPCategory).WithMany(x => x.ProjectERPs).HasForeignKey(x => x.ProjectERPCategoryId);
      builder.HasOne(x => x.ProjectERPType).WithMany(x => x.ProjectERPs).HasForeignKey(x => x.ProjectERPTypeId);
      builder.HasOne(x => x.ProjectERPPhase).WithMany(x => x.ProjectERPs).HasForeignKey(x => x.ProjectERPPhaseId);
    }
  }
}
