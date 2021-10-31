using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class PlanCodeMap : IEntityTypeConfiguration<PlanCode>
  {
    public void Configure(EntityTypeBuilder<PlanCode> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("PlanCodes");
      builder.Property(x => x.Id);
      builder.HasRowVersion();
      builder.Property(x => x.IsActive);
      builder.Property(x => x.Code);
      builder.Property(x => x.RegisterDateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.RegisterarUserId);
      builder.HasOne(x => x.RegisterarUser).WithMany(x => x.PlanCodes).HasForeignKey(x => x.RegisterarUserId);
    }
  }
}