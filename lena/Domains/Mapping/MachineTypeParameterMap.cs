using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class MachineTypeParameterMap : IEntityTypeConfiguration<MachineTypeParameter>
  {
    public void Configure(EntityTypeBuilder<MachineTypeParameter> builder)
    {
      builder.HasRowVersion();
      builder.HasKey(x => x.Id);
      builder.ToTable("MachineTypeParameters");
      builder.Property(x => x.Id);
      builder.Property(x => x.MachineTypeId);
      builder.Property(x => x.Name).IsRequired();
      builder.HasOne(x => x.MachineType).WithMany(x => x.MachineTypeParameters).HasForeignKey(x => x.MachineTypeId);
    }
  }
}
