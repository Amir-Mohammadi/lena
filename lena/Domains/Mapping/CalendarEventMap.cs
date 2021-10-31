using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class CalendarEventMap : IEntityTypeConfiguration<CalendarEvent>
  {
    public void Configure(EntityTypeBuilder<CalendarEvent> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("CalendarEvents");
      builder.Property(x => x.Id);
      builder.Property(x => x.DateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.Duration);
      builder.Property(x => x.Type);
      builder.Property(x => x.WorkShiftId);
      builder.HasRowVersion();
      builder.HasOne(x => x.WorkShift).WithMany(x => x.CalendarEvents).HasForeignKey(x => x.WorkShiftId);
      builder.HasOne(x => x.ProductionSchedule).WithOne(x => x.CalendarEvent).HasForeignKey<ProductionSchedule>(x => x.CalendarEventId);
    }
  }
}