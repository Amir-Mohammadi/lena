using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class EmployeeWorkDetailViewMap : IEntityTypeConfiguration<EmployeeWorkDetailView>
  {
    public void Configure(EntityTypeBuilder<EmployeeWorkDetailView> builder)
    {
      builder.ToTable("EmployeeWorkDetailView");
      builder.HasKey(x => x.Id);
      builder.Ignore(x => x.RowVersion);
    }
  }
}