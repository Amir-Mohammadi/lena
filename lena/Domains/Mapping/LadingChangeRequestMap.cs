using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class LadingChangeRequestMap : IEntityTypeConfiguration<LadingChangeRequest>
  {
    public void Configure(EntityTypeBuilder<LadingChangeRequest> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("LadingChangeRequests");
      builder.Property(x => x.Id);
      builder.Property(x => x.Status);
      builder.Property(x => x.UserId);
      builder.Property(x => x.ConfirmerUserId);
      builder.Property(x => x.LadingId);
      builder.Property(x => x.RequestDateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.ConfirmDateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.LadingType);
      builder.Property(x => x.Description);
      builder.HasRowVersion();
      builder.HasOne(x => x.Lading).WithMany(x => x.LadingChangeRequests).HasForeignKey(x => x.LadingId);
      ;
      builder.HasOne(x => x.User).WithMany(x => x.DemandantLadingChangeRequests).HasForeignKey(x => x.UserId);
      builder.HasOne(x => x.ConfirmerUser).WithMany(x => x.ConfirmerLadingChangeRequests).HasForeignKey(x => x.ConfirmerUserId);
    }
  }
}
