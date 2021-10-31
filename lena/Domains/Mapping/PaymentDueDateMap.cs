using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class PaymentDueDateMap : IEntityTypeConfiguration<PaymentDueDate>
  {
    public void Configure(EntityTypeBuilder<PaymentDueDate> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_PaymentDueDate");
      builder.Property(x => x.Id);
      builder.Property(x => x.OrderId);
      builder.Property(x => x.PaymentTypeId);
      builder.Property(x => x.Amount);
      builder.Property(x => x.PaymentDate).HasColumnType("smalldatetime");
      builder.HasOne(x => x.PaymentType).WithMany(x => x.PaymentDueDates).HasForeignKey(x => x.PaymentTypeId);
      builder.HasOne(x => x.Order).WithMany(x => x.PaymentDueDates).HasForeignKey(x => x.OrderId);
    }
  }
}
