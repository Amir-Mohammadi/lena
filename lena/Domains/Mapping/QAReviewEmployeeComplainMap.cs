using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class QAReviewEmployeeComplainMap : IEntityTypeConfiguration<QAReviewEmployeeComplain>
  {
    public void Configure(EntityTypeBuilder<QAReviewEmployeeComplain> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("QAReviewEmployeeComplain");
      builder.Property(x => x.EmployeeComplainItemId);
      builder.Property(x => x.ActionDescription);
      builder.Property(x => x.ActionResponsibleUserId);
      builder.Property(x => x.ActionStartDate).HasColumnType("smalldatetime");
      builder.Property(x => x.ActionFinishDate).HasColumnType("smalldatetime");
      builder.Property(x => x.ActionResult);
      builder.Property(x => x.Status);
      builder.Property(x => x.CreatorUserId);
      builder.Property(x => x.DateTime).HasColumnType("smalldatetime");
      builder.HasRowVersion();
      builder.HasOne(x => x.EmployeeComplainItem).WithMany(x => x.QAReviewEmployeeComplains).HasForeignKey(x => x.EmployeeComplainItemId);
      builder.HasOne(x => x.ResponsibleUser).WithMany(x => x.QAReviewResponsibleUserEmployeeComplains).HasForeignKey(x => x.ActionResponsibleUserId);
      builder.HasOne(x => x.CreatorUser).WithMany(x => x.QAReviewCreatorUserEmployeeComplains).HasForeignKey(x => x.CreatorUserId);
    }
  }
}
