using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class WorkStationOperationMap : IEntityTypeConfiguration<WorkStationOperation>
  {
    public void Configure(EntityTypeBuilder<WorkStationOperation> builder)
    {
      builder.HasKey(x => new
      {
        x.WorkStationId,
        x.OperationId
      });
      builder.ToTable("WorkStationOperations");
      builder.Property(x => x.WorkStationId);
      builder.Property(x => x.OperationId);
      builder.HasRowVersion();
      builder.HasOne(x => x.WorkStation).WithMany(x => x.WorkStationOperations).HasForeignKey(x => x.WorkStationId);
      builder.HasOne(x => x.Operation).WithMany(x => x.WorkStationOperations).HasForeignKey(x => x.OperationId);
    }
  }
}
