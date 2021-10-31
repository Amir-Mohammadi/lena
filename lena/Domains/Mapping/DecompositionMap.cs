using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class DecompositionMap : IEntityTypeConfiguration<Decomposition>
  {
    public void Configure(EntityTypeBuilder<Decomposition> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_Decomposition");
      builder.Property(x => x.Id);
      builder.Property(x => x.StuffId);
      builder.Property(x => x.ProductionOperationId);
      builder.Property(x => x.StuffSerialCode);
      builder.HasOne(x => x.Stuff).WithMany(x => x.Decompositions).HasForeignKey(x => x.StuffId);
      builder.HasOne(x => x.StuffSerial).WithMany(x => x.Decompositions).HasForeignKey(x => new { x.StuffSerialCode, x.StuffId });
    }
  }
}