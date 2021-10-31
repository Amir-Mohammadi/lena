using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class MachineTypeOperatorTypeMap : IEntityTypeConfiguration<MachineTypeOperatorType>
  {
    public void Configure(EntityTypeBuilder<MachineTypeOperatorType> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("MachineTypeOperatorTypes");
      builder.Property(x => x.Id).ValueGeneratedOnAdd();
      builder.Property(x => x.OperatorTypeId);
      builder.Property(x => x.MachineTypeId);
      builder.Property(x => x.Title).IsRequired();
      builder.Property(x => x.IsNecessary);
      builder.Property(x => x.IsActive);
      builder.HasRowVersion();
      builder.HasOne(x => x.MachineType).WithMany(x => x.MachineTypeOperatorTypes).HasForeignKey(x => x.MachineTypeId);
      builder.HasOne(x => x.OperatorType).WithMany(x => x.MachineTypeOperatorTypes).HasForeignKey(x => x.OperatorTypeId);
    }
  }
}
