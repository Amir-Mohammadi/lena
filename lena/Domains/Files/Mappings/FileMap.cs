using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class FileMap : IEntityTypeConfiguration<File>
  {
    public void Configure(EntityTypeBuilder<File> builder)
    {
      builder.HasKey(x => x.Id);
      builder.HasRowVersion();
      builder.Property(x => x.Id).IsRequired();
      builder.Property(x => x.FileName).IsRequired();
      builder.Property(x => x.FileType).IsRequired();
      builder.Property(x => x.FileStream).IsRequired();
    }
  }
}