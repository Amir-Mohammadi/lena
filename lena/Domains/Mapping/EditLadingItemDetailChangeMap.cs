using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class EditLadingItemDetailChangeMap : IEntityTypeConfiguration<EditLadingItemDetailChange>
  {
    public void Configure(EntityTypeBuilder<EditLadingItemDetailChange> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("EditLadingItemDetailInput");
      builder.Property(x => x.LadingIemId);
      builder.Property(x => x.Qty);
      builder.Property(x => x.CargoItemId);
      builder.Property(x => x.CargoItemDetailId);
      builder.Property(x => x.LadingItemDetailId);
      builder.Property(x => x.LadingItemDetailRowVersion);
      builder.HasRowVersion();
      builder.HasOne(x => x.LadingChangeRequest).WithMany(x => x.EditLadingItemDetailChanges).HasForeignKey(x => x.LadingChangeRequestId);
    }
  }
}
