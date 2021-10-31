using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class EntityLogMap : IEntityTypeConfiguration<EntityLog>
  {
    public void Configure(EntityTypeBuilder<EntityLog> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("EntityLogs");
      builder.Property(x => x.Ip);
      builder.Property(x => x.Api);
      builder.Property(x => x.ApiParams);
      builder.Property(x => x.DateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.Description).IsRequired(false);
      builder.HasOne(x => x.User).WithMany(x => x.EntityLogs).HasForeignKey(x => x.UserId);
    }
  }
}