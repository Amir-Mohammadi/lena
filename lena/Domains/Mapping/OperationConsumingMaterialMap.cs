using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class OperationConsumingMaterialMap : IEntityTypeConfiguration<OperationConsumingMaterial>
  {
    public void Configure(EntityTypeBuilder<OperationConsumingMaterial> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("OperationConsumingMaterials");
      builder.Property(x => x.Id);
      builder.Property(x => x.BillOfMaterialDetailId);
      builder.Property(x => x.Value);
      builder.HasRowVersion();
      builder.Property(x => x.OperationSequenceId);
      builder.Property(x => x.LimitedSerialBuffer);
      builder.HasOne(x => x.BillOfMaterialDetail).WithMany(x => x.OperationConsumingMaterials).HasForeignKey(x => x.BillOfMaterialDetailId);
      builder.HasOne(x => x.OperationSequence).WithMany(x => x.OperationConsumingMaterials).HasForeignKey(x => x.OperationSequenceId);
    }
  }
}
