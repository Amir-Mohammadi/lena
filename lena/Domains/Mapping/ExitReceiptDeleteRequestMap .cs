using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ExitReceiptDeleteRequestMap : IEntityTypeConfiguration<ExitReceiptDeleteRequest>
  {
    public void Configure(EntityTypeBuilder<ExitReceiptDeleteRequest> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("ExitReceiptDeleteRequests");
      builder.HasRemoveable();
      builder.Property(x => x.Id);
      builder.Property(x => x.Status);
      builder.Property(x => x.CreateDateTime).HasColumnType("smalldatetime").IsRequired();
      builder.Property(x => x.ChangeStatusDateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.IsDelete);
      builder.Property(x => x.CreatorUserId).IsRequired();
      builder.Property(x => x.ChangeStatusUserId);
      builder.Property(x => x.ExitReceiptId).IsRequired();
      builder.Property(x => x.Description).IsRequired();
      builder.HasRowVersion();
      builder.HasOne(x => x.CreatorUser).WithMany().HasForeignKey(x => x.CreatorUserId);
      builder.HasOne(x => x.ChangeStatusUser).WithMany().HasForeignKey(x => x.ChangeStatusUserId);
      builder.HasOne(x => x.ExitReceipt).WithMany(x => x.ExitReceiptDeleteRequests).HasForeignKey(x => x.ExitReceiptId);
    }
  }
}