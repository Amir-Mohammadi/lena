using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class BaseEntityConfirmTypeMap : IEntityTypeConfiguration<BaseEntityConfirmType>
  {
    public void Configure(EntityTypeBuilder<BaseEntityConfirmType> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntityConfirmTypes");
      builder.Property(x => x.Id);//.HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
      builder.Property(x => x.DepartmentId);
      builder.Property(x => x.UserId);
      builder.Property(x => x.ConfirmPageUrl).IsRequired();
      builder.Property(x => x.ConfirmType);
      builder.HasRowVersion();
      builder.HasOne(x => x.Department).WithMany(x => x.BaseEntityConfirmTypes).HasForeignKey(x => x.DepartmentId);
      builder.HasOne(x => x.User).WithMany(x => x.BaseEntityConfirmTypes).HasForeignKey(x => x.UserId);
    }
  }
}
