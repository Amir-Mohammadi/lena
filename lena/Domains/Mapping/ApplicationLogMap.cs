using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ApplicationLogMap : IEntityTypeConfiguration<ApplicationLog>
  {
    public void Configure(EntityTypeBuilder<ApplicationLog> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("ApplicationLogs");
      builder.Property(x => x.UserId).IsRequired(false);
      builder.Property(x => x.LogTime).HasColumnType("smalldatetime");
      builder.Property(x => x.ClientIP);
      builder.Property(x => x.UserAgent);
      builder.HasRowVersion();
      builder.Property(x => x.Action);
    }
  }
}