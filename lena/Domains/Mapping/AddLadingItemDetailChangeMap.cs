using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class AddLadingItemDetailChangeMap : IEntityTypeConfiguration<AddLadingItemDetailChange>
  {
    public void Configure(EntityTypeBuilder<AddLadingItemDetailChange> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("AddLadingItemDetailInputs");
      builder.Property(x => x.LadingIemId);
      builder.Property(x => x.Qty);
      builder.Property(x => x.CargoItemId);
      builder.Property(x => x.CargoItemDetailId);
      builder.HasRowVersion();
      builder.HasOne(x => x.LadingChangeRequest).WithMany(x => x.AddLadingItemDetailChanges).HasForeignKey(x => x.LadingChangeRequestId);
    }
  }
}