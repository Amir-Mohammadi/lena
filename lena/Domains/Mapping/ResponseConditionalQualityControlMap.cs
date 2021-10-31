using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ResponseConditionalQualityControlMap : IEntityTypeConfiguration<ResponseConditionalQualityControl>
  {
    public void Configure(EntityTypeBuilder<ResponseConditionalQualityControl> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_ResponseConditionalQualityControl");
      builder.Property(x => x.Id);
      builder.Property(x => x.Status);
      builder.Property(x => x.ConditionalQualityControlId);
      builder.HasOne(x => x.ConditionalQualityControl).WithOne(x => x.ResponseConditionalQualityControl).HasForeignKey<ResponseConditionalQualityControl>(x => x.ConditionalQualityControlId);
    }
  }
}