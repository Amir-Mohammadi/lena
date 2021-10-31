using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class StuffHSGroupMap : IEntityTypeConfiguration<StuffHSGroup>
  {
    public void Configure(EntityTypeBuilder<StuffHSGroup> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("StuffHSGroups");
      builder.HasRowVersion();
      builder.Property(x => x.Id);
      builder.Property(x => x.Code);
      builder.Property(x => x.Title);
      builder.Property(x => x.Description);
      builder.Property(x => x.RowVersion);
    }
  }
}