using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class IndicatorMap : IEntityTypeConfiguration<Indicator>
  {
    public void Configure(EntityTypeBuilder<Indicator> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("Indicators");
      builder.Property(x => x.Id);
      builder.Property(x => x.Type);
      builder.Property(x => x.Name);
      builder.Property(x => x.Target);
      builder.Property(x => x.Weight);
      builder.Property(x => x.ApiInfoId);
      builder.Property(x => x.Formula);
      builder.Property(x => x.Description);
      builder.Property(x => x.DepartmentId).IsRequired();
      builder.HasRowVersion();
      builder.HasOne(x => x.ApiInfo).WithMany(x => x.Indicators).HasForeignKey(x => x.ApiInfoId);
    }
  }
}
