using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class SerialBufferMap : IEntityTypeConfiguration<SerialBuffer>
  {
    public void Configure(EntityTypeBuilder<SerialBuffer> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("SerialBuffers");
      builder.Property(x => x.Id);
      builder.Property(x => x.RemainingAmount);
      builder.Property(x => x.ProductionTerminalId);
      builder.Property(x => x.SerialBufferType);
      builder.Property(x => x.BaseTransactionId);
      builder.HasRowVersion();
      builder.Property(x => x.DamagedAmount);
      builder.Property(x => x.ShortageAmount);
      builder.HasOne(x => x.BaseTransaction).WithOne(x => x.SerialBuffer).HasForeignKey<SerialBuffer>(x => x.BaseTransactionId);
      builder.HasOne(x => x.ProductionTerminal).WithMany(x => x.SerialBuffers).HasForeignKey(x => x.ProductionTerminalId);
    }
  }
}