using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class MachineMap : IEntityTypeConfiguration<Machine>
  {
    public void Configure(EntityTypeBuilder<Machine> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("Machines");
      builder.Property(x => x.Id).ValueGeneratedOnAdd();
      builder.Property(x => x.Name).IsRequired();
      builder.Property(x => x.Description);
      builder.HasRowVersion();
      builder.Property(x => x.MachineTypeId);
      builder.HasOne(x => x.MachineType).WithMany(x => x.Machines).HasForeignKey(x => x.MachineTypeId);
    }
  }
}
