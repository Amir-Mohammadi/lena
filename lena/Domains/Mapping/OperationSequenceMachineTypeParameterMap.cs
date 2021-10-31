using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class OperationSequenceMachineTypeParameterMap : IEntityTypeConfiguration<OperationSequenceMachineTypeParameter>
  {
    public void Configure(EntityTypeBuilder<OperationSequenceMachineTypeParameter> builder)
    {
      builder.HasRowVersion();
      builder.HasKey(x => x.Id);
      builder.ToTable("OperationSequenceMachineTypeParameters");
      builder.Property(x => x.Id);
      builder.Property(x => x.MachineTypeParameterId);
      builder.Property(x => x.OperationSequenceId);
      builder.Property(x => x.Value);
      builder.HasOne(x => x.MachineTypeParameter).WithMany(x => x.OperationSequenceMachineTypeParameters).HasForeignKey(x => x.MachineTypeParameterId);
      builder.HasOne(x => x.OperationSequence).WithMany(x => x.OperationSequenceMachineTypeParameters).HasForeignKey(x => x.OperationSequenceId);
    }
  }
}
