using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class CalendarMap : IEntityTypeConfiguration<Calendar>
  {
    public void Configure(EntityTypeBuilder<Calendar> builder)
    {
      builder.HasKey(x => x.Date);
      builder.ToTable("Calendars");
      builder.Property(x => x.Date).HasColumnType("smalldatetime");
      builder.Property(x => x.IsWorkingDay);
      builder.Property(x => x.IsHoliday);
      builder.HasRowVersion();
    }
  }
}