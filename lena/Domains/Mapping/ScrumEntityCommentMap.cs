using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ScrumEntityCommentMap : IEntityTypeConfiguration<ScrumEntityComment>
  {
    public void Configure(EntityTypeBuilder<ScrumEntityComment> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("ScrumEntityComments");
      builder.Property(x => x.Id);
      builder.Property(x => x.DateTime).HasColumnType("smalldatetime").IsRequired();
      builder.Property(x => x.Comment).IsRequired();
      builder.Property(x => x.UserId);
      builder.Property(x => x.ScrumEntityId);
      builder.HasRowVersion();
      builder.HasOne(x => x.User).WithMany(x => x.ScrumEntityComments).HasForeignKey(x => x.UserId);
      builder.HasOne(x => x.ScrumEntity).WithMany(x => x.ScrumEntityComments).HasForeignKey(x => x.ScrumEntityId);
    }
  }
}
