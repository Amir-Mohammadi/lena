using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ResponseWarehouseIssueMap : IEntityTypeConfiguration<ResponseWarehouseIssue>
  {
    public void Configure(EntityTypeBuilder<ResponseWarehouseIssue> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_ResponseWarehouseIssue");
      builder.Property(x => x.Id);
      builder.Property(x => x.WarehouseIssueId);
      builder.HasOne(x => x.WarehouseIssue).WithOne(x => x.ResponseWarehouseIssue).HasForeignKey<ResponseWarehouseIssue>(x => x.WarehouseIssueId);
    }
  }
}