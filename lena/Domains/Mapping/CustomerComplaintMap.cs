using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class CustomerComplaintMap : IEntityTypeConfiguration<CustomerComplaint>
  {
    public void Configure(EntityTypeBuilder<CustomerComplaint> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("CustomerComplaints");
      builder.Property(x => x.Id);
      builder.HasRowVersion();
      builder.Property(x => x.CustomerId);
      builder.Property(x => x.RegisterarUserId);
      builder.Property(x => x.DateOfComplaint);
      builder.Property(x => x.ResponseDeadline);
      builder.Property(x => x.ComplaintTypeDescription);
      builder.Property(x => x.ComplaintTypes);
      builder.Property(x => x.RegisterarDateTime).HasColumnType("smalldatetime");
      builder.HasOne(x => x.Customer).WithMany(x => x.CustomerComplaints).HasForeignKey(x => x.CustomerId);
      builder.HasOne(x => x.RegisterarUser).WithMany(x => x.CustomerComplaints).HasForeignKey(x => x.RegisterarUserId);
    }
  }
}
