﻿using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class TestEquipmentMap : IEntityTypeConfiguration<TestEquipment>
  {
    public void Configure(EntityTypeBuilder<TestEquipment> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("TestEquipments");
      builder.Property(x => x.Id);
      builder.Property(x => x.Name);
      builder.Property(x => x.Description);
      builder.HasRowVersion();
    }
  }
}
