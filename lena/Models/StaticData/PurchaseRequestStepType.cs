using System;

using lena.Domains;
namespace lena.Models.StaticData
{
  public static class PurchaseRequestStepType
  {
    static PurchaseRequestStepType()
    {
      SearchSupplier = new PurchaseRequestStep()
      {
        Id = 1,
        Name = "جستجوی تامین کننده",
        AllowUploadDocument = false,
        IsActive = true,
        DateTime = DateTime.UtcNow,
        UserId = 20004 //ماشین
      };
      WaitingForSupplierResponse = new PurchaseRequestStep()
      {
        Id = 2,
        Name = "منتظر پاسخ تامین کننده",
        AllowUploadDocument = true,
        IsActive = true,
        DateTime = DateTime.UtcNow,
        UserId = 20004 //ماشین
      };

      WaitingForTechnicalSpecifications = new PurchaseRequestStep()
      {
        Id = 3,
        Name = "منتظر مشخصات فنی",
        AllowUploadDocument = true,
        IsActive = true,
        DateTime = DateTime.UtcNow,
        UserId = 20004 //ماشین
      };

      WaitingForInvoice = new PurchaseRequestStep()
      {
        Id = 4,
        Name = "منتظر پیش فاکتور",
        AllowUploadDocument = false,
        IsActive = true,
        DateTime = DateTime.UtcNow,
        UserId = 20004 //ماشین
      };

      Warning = new PurchaseRequestStep()
      {
        Id = 5,
        Name = "هشدار",
        AllowUploadDocument = false,
        IsActive = true,
        DateTime = DateTime.UtcNow,
        UserId = 20004 //ماشین
      };

    }
    public static lena.Domains.PurchaseRequestStep SearchSupplier { get; }
    public static lena.Domains.PurchaseRequestStep WaitingForSupplierResponse { get; }
    public static lena.Domains.PurchaseRequestStep WaitingForTechnicalSpecifications { get; }
    public static lena.Domains.PurchaseRequestStep WaitingForInvoice { get; }
    public static lena.Domains.PurchaseRequestStep Warning { get; }

  }
}
