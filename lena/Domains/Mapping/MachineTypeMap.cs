using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class MachineTypeMap : IEntityTypeConfiguration<MachineType>
  {
    public void Configure(EntityTypeBuilder<MachineType> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("WorkStationParts_MachineType");
      builder.Property(x => x.Id);
    }
  }
}
