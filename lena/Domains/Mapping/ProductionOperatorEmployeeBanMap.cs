using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ProductionOperatorEmployeeBanMap : IEntityTypeConfiguration<ProductionOperatorEmployeeBan>
  {
    public void Configure(EntityTypeBuilder<ProductionOperatorEmployeeBan> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("ProductionOperatorEmployeeBans");
      builder.Property(x => x.Id);
      builder.Property(x => x.ProductionOperatorId).IsRequired();
      builder.Property(x => x.EmployeeId).IsRequired();
      builder.Property(x => x.BanDateTime).HasColumnType("smalldatetime").IsRequired();
      builder.Property(x => x.IsBan).IsRequired();
      builder.Property(x => x.RevokeDateTime).HasColumnType("smalldatetime").IsRequired(false);
      builder.Property(x => x.RevokeUserId).IsRequired(false);
      builder.HasRowVersion();
      builder.HasOne(x => x.ProductionOperator).WithMany(x => x.ProductionOperatorEmployeeBans).HasForeignKey(x => x.ProductionOperatorId);
      builder.HasOne(x => x.Employee).WithMany(x => x.ProductionOperatorEmployeeBans).HasForeignKey(x => x.EmployeeId);
      builder.HasOne(x => x.User).WithMany(x => x.ProductionOperatorEmployeeBans).HasForeignKey(x => x.RevokeUserId);
    }
  }
}
