using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class TestConditionMap : IEntityTypeConfiguration<TestCondition>
  {
    public void Configure(EntityTypeBuilder<TestCondition> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("TestConditions");
      builder.Property(x => x.Condition);
      builder.Property(x => x.UserId);
      builder.Property(x => x.DateTime).HasColumnType("smalldatetime");
      builder.HasRowVersion();
    }
  }
}
