using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ScrumEntityMap : IEntityTypeConfiguration<ScrumEntity>
  {
    public void Configure(EntityTypeBuilder<ScrumEntity> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("ScrumEntities");
      builder.Property(x => x.Id);
      builder.Property(x => x.Code).IsRequired();
      builder.Property(x => x.Name).IsRequired();
      builder.Property(x => x.Description);
      builder.Property(x => x.Color).IsRequired();
      builder.Property(x => x.IsCommit);
      builder.Property(x => x.EstimatedTime);
      builder.Property(x => x.DepartmentId);
      builder.Property(x => x.DateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.IsDelete);
      builder.Property(x => x.IsArchive);
      builder.HasRowVersion();
      builder.Property(x => x.BaseEntityId);
      builder.HasOne(x => x.Department).WithMany(x => x.ScrumEntities).HasForeignKey(x => x.DepartmentId);
      builder.HasOne(x => x.BaseEntity).WithMany(x => x.ScrumEntities).HasForeignKey(x => x.BaseEntityId);
    }
  }
}