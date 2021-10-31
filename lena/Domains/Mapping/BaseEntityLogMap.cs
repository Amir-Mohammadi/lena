using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class BaseEntityLogMap : IEntityTypeConfiguration<BaseEntityLog>
  {
    public void Configure(EntityTypeBuilder<BaseEntityLog> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntityLogs");
      builder.Property(x => x.Id);
      builder.Property(x => x.BaseEntityId);
      builder.Property(x => x.UserId);
      builder.Property(x => x.DateTime).HasColumnType("smalldatetime");
      builder.HasRowVersion();
      builder.HasOne(x => x.BaseEntity).WithMany(x => x.BaseEntityLogs).HasForeignKey(x => x.BaseEntityId);
      builder.HasOne(x => x.User).WithMany(x => x.BaseEntityLogs).HasForeignKey(x => x.UserId);
    }
  }
}
