using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class WorkStationPartMap : IEntityTypeConfiguration<WorkStationPart>
  {
    public void Configure(EntityTypeBuilder<WorkStationPart> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("WorkStationParts");
      builder.Property(x => x.Id).ValueGeneratedOnAdd();
      builder.Property(x => x.Name).IsRequired();
      builder.Property(x => x.WorkStationId);
      builder.Property(x => x.Description);
      builder.HasRowVersion();
      builder.HasOne(x => x.WorkStation).WithMany(x => x.WorkStationParts).HasForeignKey(x => x.WorkStationId);
    }
  }
}