using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class BaseEntityConfirmationMap : IEntityTypeConfiguration<BaseEntityConfirmation>
  {
    public void Configure(EntityTypeBuilder<BaseEntityConfirmation> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_BaseEntityConfirmation");
      builder.Property(x => x.Id);
      builder.Property(x => x.Status);
      builder.Property(x => x.ConfirmDescription);
      builder.Property(x => x.BaseEntityConfirmTypeId);
      builder.Property(x => x.ConfirmerId);
      builder.Property(x => x.ConfirmingEntityId);
      builder.Property(x => x.ConfirmDateTime).HasColumnType("smalldatetime");
      builder.HasOne(x => x.BaseEntityConfirmType).WithMany(x => x.BaseEntityConfirmations).HasForeignKey(x => x.BaseEntityConfirmTypeId);
      builder.HasOne(x => x.Confirmer).WithMany(x => x.BaseEntityConfirmations).HasForeignKey(x => x.ConfirmerId);
      builder.HasOne(x => x.ConfirmingEntity).WithMany(x => x.BaseEntityConfirmations).HasForeignKey(x => x.ConfirmingEntityId);
    }
  }
}
