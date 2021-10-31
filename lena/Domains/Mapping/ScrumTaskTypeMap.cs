using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ScrumTaskTypeMap : IEntityTypeConfiguration<ScrumTaskType>
  {
    public void Configure(EntityTypeBuilder<ScrumTaskType> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("ScrumTaskTypes");
      builder.Property(x => x.Id);
      builder.Property(x => x.Name).IsRequired();
      builder.HasRowVersion();
    }
  }
}
