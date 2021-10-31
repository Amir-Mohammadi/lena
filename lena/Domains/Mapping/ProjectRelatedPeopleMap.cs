using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ProjectRelatedPeopleMap : IEntityTypeConfiguration<ProjectRelatedPeople>
  {
    public void Configure(EntityTypeBuilder<ProjectRelatedPeople> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("ProjectRelatedPeoples");
      builder.Property(x => x.Id);
      builder.Property(x => x.Post).IsRequired();
      builder.Property(x => x.Name).IsRequired();
      builder.Property(x => x.PhoneNumber).IsRequired();
      builder.Property(x => x.Description).IsRequired();
      builder.Property(x => x.ProjectId);
      builder.HasRowVersion();
      builder.HasOne(x => x.Project).WithMany(x => x.ProjectRelatedPeoples).HasForeignKey(x => x.ProjectId);
    }
  }
}
