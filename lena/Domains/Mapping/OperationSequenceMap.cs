using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class OperationSequenceMap : IEntityTypeConfiguration<OperationSequence>
  {
    public void Configure(EntityTypeBuilder<OperationSequence> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("OperationSequences");
      builder.Property(x => x.Id);
      builder.Property(x => x.Index);
      builder.Property(x => x.DefaultTime);
      builder.Property(x => x.IsOptional);
      builder.Property(x => x.IsRepairReturnPoint);
      builder.Property(x => x.WorkPlanStepId);
      builder.Property(x => x.WorkStationPartId);
      builder.Property(x => x.WorkStationPartCount);
      builder.Property(x => x.Description);
      builder.Property(x => x.WorkStationId);
      builder.Property(x => x.OperationId);
      builder.HasRowVersion();
      builder.HasOne(x => x.WorkStationPart).WithMany(x => x.OperationSequences).HasForeignKey(x => x.WorkStationPartId);
      builder.HasOne(x => x.WorkPlanStep).WithMany(x => x.OperationSequences).HasForeignKey(x => x.WorkPlanStepId);
      builder.HasOne(x => x.WorkStationOperation).WithMany(x => x.OperationSequences).HasForeignKey(x => new { x.WorkStationId, x.OperationId });
      builder.HasOne(x => x.Operation).WithMany(x => x.OperationSequences).HasForeignKey(x => x.OperationId);
      builder.HasOne(x => x.WorkStation).WithMany(x => x.OperationSequences).HasForeignKey(x => x.WorkStationId);
    }
  }
}