using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ApiInfoMap : IEntityTypeConfiguration<ApiInfo>
  {
    public void Configure(EntityTypeBuilder<ApiInfo> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("ApiInfos");
      builder.Property(x => x.Id);
      builder.Property(x => x.Url);
      builder.Property(x => x.Name);
      builder.Property(x => x.Param);
      builder.Property(x => x.SortTypeName);
      builder.Property(x => x.SortTypeFieldName);
      builder.HasRowVersion();
    }
  }
}