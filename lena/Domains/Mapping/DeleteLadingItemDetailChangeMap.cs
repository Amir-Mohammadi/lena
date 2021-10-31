using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class DeleteLadingItemDetailChangeMap : IEntityTypeConfiguration<DeleteLadingItemDetailChange>
  {
    public void Configure(EntityTypeBuilder<DeleteLadingItemDetailChange> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("DeleteLadingItemDetailInput");
      builder.Property(x => x.LadingIemId);
      builder.Property(x => x.CargoItemId);
      builder.Property(x => x.LadingItemDetailId);
      builder.Property(x => x.LadingItemDetailRowVersion);
      builder.HasRowVersion();
      builder.HasOne(x => x.LadingChangeRequest).WithMany(x => x.DeleteLadingItemDetailChanges).HasForeignKey(x => x.LadingChangeRequestId);
    }
  }
}
