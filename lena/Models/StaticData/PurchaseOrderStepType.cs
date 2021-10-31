using System;

using lena.Domains;
namespace lena.Models.StaticData
{
  public static class PurchaseOrderStepType
  {
    static PurchaseOrderStepType()
    {
      WaitingForPrePayment = new PurchaseOrderStep()
      {
        Id = 1,
        Name = "منتظر پیش پرداخت",
        AllowUploadDocument = false,
        IsActive = true,
        DateTime = DateTime.UtcNow,
        UserId = 20004 //ماشین
      };
      FollowConstructionPhase = new PurchaseOrderStep()
      {
        Id = 2,
        Name = "پیگیری مرحله ساخت",
        AllowUploadDocument = true,
        IsActive = true,
        DateTime = DateTime.UtcNow,
        UserId = 20004 //ماشین
      };

      WaitingForPayment = new PurchaseOrderStep()
      {
        Id = 3,
        Name = "منتظر پرداخت",
        AllowUploadDocument = false,
        IsActive = true,
        DateTime = DateTime.UtcNow,
        UserId = 20004 //ماشین
      };

      WaitingForDebtToBePaid = new PurchaseOrderStep()
      {
        Id = 4,
        Name = "منتظر پرداخت بدهی",
        AllowUploadDocument = false,
        IsActive = true,
        DateTime = DateTime.UtcNow,
        UserId = 20004 //ماشین
      };

      SendToForwarder = new PurchaseOrderStep()
      {
        Id = 5,
        Name = "ارسال به فورواردر",
        AllowUploadDocument = true,
        IsActive = true,
        DateTime = DateTime.UtcNow,
        UserId = 20004 //ماشین
      };

      ReceivedFromForwarder = new PurchaseOrderStep()
      {
        Id = 6,
        Name = "دریافت توسط فورواردر",
        AllowUploadDocument = false,
        IsActive = true,
        DateTime = DateTime.UtcNow,
        UserId = 20004 //ماشین
      };

      Warning = new PurchaseOrderStep()
      {
        Id = 7,
        Name = "هشدار",
        AllowUploadDocument = false,
        IsActive = true,
        DateTime = DateTime.UtcNow,
        UserId = 20004 //ماشین
      };

    }
    public static lena.Domains.PurchaseOrderStep WaitingForPrePayment { get; }
    public static lena.Domains.PurchaseOrderStep FollowConstructionPhase { get; }
    public static lena.Domains.PurchaseOrderStep WaitingForPayment { get; }
    public static lena.Domains.PurchaseOrderStep WaitingForDebtToBePaid { get; }
    public static lena.Domains.PurchaseOrderStep SendToForwarder { get; }
    public static lena.Domains.PurchaseOrderStep ReceivedFromForwarder { get; }
    public static lena.Domains.PurchaseOrderStep Warning { get; }

  }
}
