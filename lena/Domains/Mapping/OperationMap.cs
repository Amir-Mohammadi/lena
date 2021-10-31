using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class OperationMap : IEntityTypeConfiguration<Operation>
  {
    public void Configure(EntityTypeBuilder<Operation> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("Operations");
      builder.Property(x => x.Id).ValueGeneratedOnAdd();
      builder.Property(x => x.Title).IsRequired();
      builder.Property(x => x.Code).IsRequired();
      builder.Property(x => x.Description);
      builder.Property(x => x.OperationTypeId);
      builder.HasRowVersion();
      builder.Property(x => x.IsQualityControl);
      builder.Property(x => x.IsCorrective);
      builder.HasOne(x => x.OperationType).WithMany(x => x.Operations).HasForeignKey(x => x.OperationTypeId);
    }
  }
}
