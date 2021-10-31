using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class WorkShiftMap : IEntityTypeConfiguration<WorkShift>
  {
    public void Configure(EntityTypeBuilder<WorkShift> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("WorkShifts");
      builder.Property(x => x.Id);
      builder.Property(x => x.Name).IsRequired();
      builder.HasRowVersion();
    }
  }
}
