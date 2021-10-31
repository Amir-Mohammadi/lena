using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class DetailedCodeConfirmationRequestMap : IEntityTypeConfiguration<DetailedCodeConfirmationRequest>
  {
    public void Configure(EntityTypeBuilder<DetailedCodeConfirmationRequest> builder)
    {
      builder.HasKey(m => m.Id);
      builder.ToTable("DetailedCodeConfirmationRequests");
      builder.Property(m => m.Id);
      builder.Property(m => m.CooperatorId);
      builder.Property(m => m.ProductionLineId);
      builder.Property(m => m.DetailedCodeRequestType);
      builder.Property(m => m.DateTime).HasColumnType("smalldatetime");
      builder.Property(m => m.UserId);
      builder.Property(m => m.ConfirmationDateTime);
      builder.Property(m => m.ConfirmationUserId);
      builder.Property(m => m.Status);
      builder.Property(m => m.DetailedCodeEntityType);
      builder.Property(m => m.Description);
      builder.Property(m => m.DetailedCode);
      builder.Property(m => m.RowVersion).IsRowVersion();
      builder.HasOne(m => m.Cooperator).WithMany(m => m.DetailedCodeConfirmationRequests).HasForeignKey(d => d.CooperatorId);
      builder.HasOne(m => m.ProductionLine).WithMany(m => m.DetailedCodeConfirmationRequests).HasForeignKey(d => d.ProductionLineId);
      builder.HasOne(m => m.User).WithMany(m => m.DetailedCodeConfirmationRequest).HasForeignKey(m => m.UserId);
      builder.HasOne(m => m.DetailedCodeConfirmerUser).WithMany(m => m.ConfirmerDetailedCodeConfirmationRequest).HasForeignKey(m => m.ConfirmationUserId);
    }
  }
}