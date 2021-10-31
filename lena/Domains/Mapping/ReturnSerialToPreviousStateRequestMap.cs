using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ReturnSerialToPreviousStateRequestMap : IEntityTypeConfiguration<ReturnSerialToPreviousStateRequest>
  {
    public void Configure(EntityTypeBuilder<ReturnSerialToPreviousStateRequest> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("ReturnSerialToPreviousStateRequests");
      builder.Property(x => x.Id);
      builder.Property(x => x.Serial);
      builder.Property(x => x.StuffId);
      builder.Property(x => x.StuffSerialCode);
      builder.Property(x => x.UserId);
      builder.Property(x => x.ConfirmerUserId);
      builder.Property(x => x.WrongDoerUserId);
      builder.Property(x => x.RequestDateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.ConfirmDateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.Description);
      builder.Property(x => x.Status);
      builder.Property(x => x.RowVersion).IsRowVersion().IsRequired();
      builder.HasOne(x => x.StuffSerial).WithMany(x => x.ReturnSerialToPreviousStateRequests).HasForeignKey(x => new
      {
        x.StuffSerialCode,
        x.StuffId
      });
      builder.HasOne(x => x.User).WithMany(x => x.DemandantSerialReturnRequests).HasForeignKey(x => x.UserId);
      builder.HasOne(x => x.ConfirmerUser).WithMany(x => x.ConfirmerSerilReturnRequests).HasForeignKey(x => x.ConfirmerUserId);
      builder.HasOne(x => x.WrongDoerUser).WithMany(x => x.WrongDoerSerilReturnRequests).HasForeignKey(x => x.WrongDoerUserId);
    }
  }
}