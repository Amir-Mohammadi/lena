using lena.Domains;
using lena.Domains;
namespace lena.Models.StaticData
{
  public static class StaticExitReceiptRequestTypes
  {
    static StaticExitReceiptRequestTypes()
    {
      OrderExitReceiptRequest = new ExitReceiptRequestType()
      {
        Id = 1,
        IsActive = true,
        Title = "ارسال سفارش",
        AutoConfirm = false
      };
      SaleExitReceiptRequest = new ExitReceiptRequestType()
      {
        Id = 2,
        IsActive = true,
        Title = "فروش",
        AutoConfirm = false
      };
      LendingExitReceiptRequest = new ExitReceiptRequestType()
      {
        Id = 3,
        IsActive = true,
        Title = "ارسال خدمات امانی",
        AutoConfirm = false
      };
      SampleExitReceiptRequest = new ExitReceiptRequestType()
      {
        Id = 4,
        IsActive = true,
        Title = "نمونه",
        AutoConfirm = false
      };
      GiftExitReceiptRequest = new ExitReceiptRequestType()
      {
        Id = 5,
        IsActive = true,
        Title = "هدیه",
        AutoConfirm = false
      };
      GivebackExitReceiptRequest = new ExitReceiptRequestType()
      {
        Id = 6,
        IsActive = true,
        Title = "مرجوعی",
        AutoConfirm = false
      };
      DisposalOfWasteExitReceiptRequest = new ExitReceiptRequestType()
      {
        Id = 7,
        IsActive = true,
        Title = "امحای ضایعات",
        AutoConfirm = false
      };
      AfteSalesServices = new ExitReceiptRequestType()
      {
        Id = 8,
        IsActive = true,
        Title = "خدمات پس از فروش",
        AutoConfirm = false
      };
      ReturnedExitReceiptRequest = new ExitReceiptRequestType()
      {
        Id = 9,
        IsActive = true,
        Title = "برگشت از فروش",
        AutoConfirm = false
      };
      NoGuaranteeReceiptRequest = new ExitReceiptRequestType()
      {
        Id = 10,
        IsActive = true,
        Title = "بدون گارانتی",
        AutoConfirm = false
      };
    }
    public static ExitReceiptRequestType OrderExitReceiptRequest { get; }
    public static ExitReceiptRequestType SaleExitReceiptRequest { get; }
    public static ExitReceiptRequestType LendingExitReceiptRequest { get; }
    public static ExitReceiptRequestType SampleExitReceiptRequest { get; }
    public static ExitReceiptRequestType GiftExitReceiptRequest { get; }
    public static ExitReceiptRequestType GivebackExitReceiptRequest { get; }
    public static ExitReceiptRequestType DisposalOfWasteExitReceiptRequest { get; }
    public static ExitReceiptRequestType ReturnedExitReceiptRequest { get; }
    public static ExitReceiptRequestType AfteSalesServices { get; }
    public static ExitReceiptRequestType NoGuaranteeReceiptRequest { get; }
  }
}