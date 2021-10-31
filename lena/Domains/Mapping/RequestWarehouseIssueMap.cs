using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class RequestWarehouseIssueMap : IEntityTypeConfiguration<RequestWarehouseIssue>
  {
    public void Configure(EntityTypeBuilder<RequestWarehouseIssue> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_RequestWarehouseIssue");
      builder.Property(x => x.Id);
    }
  }
}
