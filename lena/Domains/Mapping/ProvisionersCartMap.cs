using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ProvisionersCartMap : IEntityTypeConfiguration<ProvisionersCart>
  {
    public void Configure(EntityTypeBuilder<ProvisionersCart> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("ProvisionersCarts");
      builder.Property(x => x.Id);
      builder.HasRowVersion();
      builder.Property(x => x.Status);
      builder.Property(x => x.DateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.Description);
      builder.Property(x => x.ReportDate).HasColumnType("smalldatetime");
      builder.Property(x => x.SupplierId);
      builder.Property(x => x.ResponsibleEmployeeId);
      builder.HasOne(x => x.ResponsibleEmployee).WithMany(x => x.ProvisionersCarts).HasForeignKey(x => x.ResponsibleEmployeeId);
      builder.HasOne(x => x.Supplier).WithMany(x => x.ProvisionersCarts).HasForeignKey(x => x.SupplierId);
    }
  }
}