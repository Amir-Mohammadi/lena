using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ProductionPerformanceInfoMap : IEntityTypeConfiguration<ProductionPerformanceInfo>
  {
    public void Configure(EntityTypeBuilder<ProductionPerformanceInfo> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("ProductionPerformanceInfoes");
      builder.HasRowVersion();
      builder.Property(x => x.DescriptionDateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.Description);
      builder.Property(x => x.ResponsibleComment);
      builder.Property(x => x.CorrectiveAction);
      builder.Property(x => x.Status);
      builder.Property(x => x.RegistrationDate).HasColumnType("smalldatetime");
      builder.Property(x => x.ConfirmationDate).HasColumnType("smalldatetime");
      builder.Property(x => x.RegistratorUserId);
      builder.Property(x => x.ConfirmatorUserId);
      builder.Property(x => x.ProductionOrderId);
      builder.Property(x => x.DepartmentId);
      builder.HasOne(x => x.ProductionOrder).WithMany(x => x.ProductionPerformanceInfoes).HasForeignKey(x => x.ProductionOrderId);
      builder.HasOne(x => x.Department).WithMany(x => x.ProductionPerformanceInfoes).HasForeignKey(x => x.DepartmentId);
      builder.HasOne(x => x.ConfirmatorUser).WithMany().HasForeignKey(x => x.ConfirmatorUserId);
      builder.HasOne(x => x.RegistratorUser).WithMany().HasForeignKey(x => x.RegistratorUserId);
    }
  }
}