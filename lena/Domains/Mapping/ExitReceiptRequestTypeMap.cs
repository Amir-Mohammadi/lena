using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ExitReceiptRequestTypeMap : IEntityTypeConfiguration<ExitReceiptRequestType>
  {
    public void Configure(EntityTypeBuilder<ExitReceiptRequestType> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("ExitReceiptRequestTypes");
      builder.Property(x => x.Id);//.HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
      builder.Property(x => x.Title).IsRequired();
      builder.Property(x => x.AutoConfirm);
      builder.Property(x => x.IsActive);
      builder.Property(x => x.Description);
      builder.HasRowVersion();
    }
  }
}
