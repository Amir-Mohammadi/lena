using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ProjectERPEventMap : IEntityTypeConfiguration<ProjectERPEvent>
  {
    public void Configure(EntityTypeBuilder<ProjectERPEvent> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("ProjectERPEvents");
      builder.Property(x => x.Id);
      builder.Property(x => x.ProjectERPId);
      builder.Property(x => x.ProjectERPEventCategoryId);
      builder.Property(x => x.RegisterUserId);
      builder.Property(x => x.AudienceEmployeeId).IsRequired(false);
      //builder.Property(x => x.ProjectERPEventDocumentId);
      builder.Property(x => x.RegisterDateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.NextActionDateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.CRMDateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.Audience);
      // مخاطب      builder.Property(x => x.Confidential);
      builder.Property(x => x.NextAction);
      builder.Property(x => x.Description);
      builder.Property(x => x.AnnouncementType).IsRequired(false);
      builder.Property(x => x.AnnouncementDescription);
      builder.Property(x => x.Type);
      //builder.Property(x => x.ProjectERPEventActionTypeId);
      builder.Property(x => x.RowVersion).IsRowVersion().IsRequired();
      builder.HasOne(x => x.RegisterUser).WithMany(x => x.ProjectERPEventRecords).HasForeignKey(x => x.RegisterUserId);
      builder.HasOne(x => x.AudienceEmployee).WithMany(x => x.ProjectERPEvents).HasForeignKey(x => x.AudienceEmployeeId);
      builder.HasOne(x => x.ProjectERPEventCategory).WithMany(x => x.ProjectERPEvents).HasForeignKey(x => x.ProjectERPEventCategoryId);
      //builder.HasOne(x => x.ProjectERPEventActionType).WithMany(x => x.ProjectERPEvents).HasForeignKey(x => x.ProjectERPEventActionTypeId);
      builder.HasOne(x => x.ProjectERP).WithMany(x => x.ProjectERPEvents).HasForeignKey(x => x.ProjectERPId);
    }
  }
}