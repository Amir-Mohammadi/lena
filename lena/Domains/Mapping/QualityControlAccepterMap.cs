using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class QualityControlAccepterMap : IEntityTypeConfiguration<QualityControlAccepter>
  {
    public void Configure(EntityTypeBuilder<QualityControlAccepter> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("QualityControlAccepters");
      builder.Property(x => x.Id);
      builder.Property(x => x.Title).IsRequired();
      builder.HasRowVersion();
      builder.Property(x => x.UserGroupId);
      builder.HasOne(x => x.UserGroup).WithMany(x => x.QualityControlAccepters).HasForeignKey(x => x.UserGroupId);
    }
  }
}
