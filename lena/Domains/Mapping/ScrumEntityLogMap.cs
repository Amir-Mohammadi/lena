using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ScrumEntityLogMap : IEntityTypeConfiguration<ScrumEntityLog>
  {
    public void Configure(EntityTypeBuilder<ScrumEntityLog> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("ScrumEntityLogs");
      builder.Property(x => x.Id);
      builder.Property(x => x.ScrumEntityId);
      builder.Property(x => x.FieldName).IsRequired();
      builder.Property(x => x.OldValue);
      builder.Property(x => x.NewValue);
      builder.Property(x => x.DateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.UserId);
      builder.Property(x => x.Description);
      builder.HasRowVersion();
      builder.HasOne(x => x.ScrumEntity).WithMany(x => x.ScrumEntityLogs).HasForeignKey(x => x.ScrumEntityId);
      builder.HasOne(x => x.User).WithMany(x => x.ScrumEntityLogs).HasForeignKey(x => x.UserId);
    }
  }
}
