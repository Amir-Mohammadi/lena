using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ScrumBackLogMap : IEntityTypeConfiguration<ScrumBackLog>
  {
    public void Configure(EntityTypeBuilder<ScrumBackLog> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("ScrumEntities_ScrumBackLog");
      builder.Property(x => x.Id);
      builder.Property(x => x.ScrumSprintId);
      builder.HasOne(x => x.ScrumSprint).WithMany(x => x.ScrumBackLogs).HasForeignKey(x => x.ScrumSprintId);
    }
  }
}
