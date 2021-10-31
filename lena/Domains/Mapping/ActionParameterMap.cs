using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ActionParameterMap : IEntityTypeConfiguration<ActionParameter>
  {
    public void Configure(EntityTypeBuilder<ActionParameter> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("ActionParamater");
      builder.Property(x => x.Id);
      builder.Property(x => x.SecurityActionId);
      builder.Property(x => x.ParameterKey).IsRequired();
      builder.Property(x => x.ParameterValue);
      builder.Property(x => x.CheckParameterValue);
      builder.HasRowVersion();
    }
  }
}
