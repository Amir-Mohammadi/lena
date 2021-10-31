using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class BankOrderMap : IEntityTypeConfiguration<BankOrder>
  {
    public void Configure(EntityTypeBuilder<BankOrder> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_BankOrder");
      builder.Property(x => x.Id);
      builder.Property(x => x.OrderNumber).IsRequired();
      builder.Property(x => x.FolderCode).IsRequired();
      builder.Property(x => x.StuffPriority).IsRequired();
      builder.Property(x => x.RegisterDate).HasColumnType("smalldatetime");
      builder.Property(x => x.ExpireDate).HasColumnType("smalldatetime");
      builder.Property(x => x.SettlementDateTime).HasColumnType("smalldatetime").IsRequired(false);
      builder.Property(x => x.ProviderId);
      builder.Property(x => x.CurrencyId);
      builder.Property(x => x.Status);
      builder.Property(x => x.BankId);
      builder.Property(x => x.CustomhouseId);
      builder.Property(x => x.CountryId);
      builder.Property(x => x.TransferCost);
      builder.Property(x => x.FOB);
      builder.Property(x => x.TotalAmount);
      builder.Property(x => x.BankOrderContractTypeId);
      builder.HasOne(x => x.Provider).WithMany(x => x.BankOrders).HasForeignKey(x => x.ProviderId);
      builder.HasOne(x => x.Currency).WithMany(x => x.BankOrders).HasForeignKey(x => x.CurrencyId);
      builder.HasOne(x => x.Bank).WithMany(x => x.BankOrders).HasForeignKey(x => x.BankId);
      builder.HasOne(x => x.Customhouse).WithMany(x => x.BankOrders).HasForeignKey(x => x.CustomhouseId);
      builder.HasOne(x => x.Country).WithMany(x => x.BankOrders).HasForeignKey(x => x.CountryId);
      builder.HasOne(x => x.BankOrderContractType).WithMany(x => x.BankOrders).HasForeignKey(x => x.BankOrderContractTypeId);
      builder.HasOne(x => x.Enactment).WithOne(x => x.BankOrder).HasForeignKey<Enactment>(x => x.BankOrderId);
    }
  }
}