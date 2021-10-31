using lena.Domains;
using lena.Domains.Enums;
namespace lena.Models.StaticData
{
  public static class StaticTransactionTypes
  {
    static StaticTransactionTypes()
    {
      ExportAsDirectStoreIssue = new TransactionType()
      {
        Id = 0,
        Name = "خروج حواله مستقیم ",
        Factor = TransactionTypeFactor.Minus,
        TransactionLevel = TransactionLevel.Available
      };
      ImportAsDirectStoreIssue = new TransactionType()
      {
        Id = 1,
        Name = "ورود حواله مستقیم",
        Factor = TransactionTypeFactor.Plus,
        TransactionLevel = TransactionLevel.Available
      };
      ExportAsIndirectStoreIssue = new TransactionType()
      {
        Id = 2,
        Name = "خروج حواله غیر مستقیم",
        Factor = TransactionTypeFactor.Minus,
        TransactionLevel = TransactionLevel.Available
      };
      ImportAsBlockedStoreIssue = new TransactionType()
      {
        Id = 3,
        Name = "ورود حواله تایید نشده",
        Factor = TransactionTypeFactor.Plus,
        TransactionLevel = TransactionLevel.Blocked
      };
      ExportAsConfirmedBlockedStoreIssue = new TransactionType()
      {
        Id = 4,
        Name = "خروج حواله تایید شده ",
        Factor = TransactionTypeFactor.Minus,
        TransactionLevel = TransactionLevel.Blocked
      };
      ExportAsRejectedBlockedStoreIssue = new TransactionType()
      {
        Id = 5,
        Name = "بازگشت حواله تایید نشده",
        Factor = TransactionTypeFactor.Minus,
        TransactionLevel = TransactionLevel.Blocked
      };
      ImportAsIndirectStoreIssue = new TransactionType()
      {
        Id = 6,
        Name = "ورود حواله غیر مستقیم",
        Factor = TransactionTypeFactor.Plus,
        TransactionLevel = TransactionLevel.Available
      };
      ImportBlock = new TransactionType()
      {
        Id = 7,
        Name = "ورود به سطح بلوکه",
        Factor = TransactionTypeFactor.Plus,
        TransactionLevel = TransactionLevel.Blocked
      };
      ExportBlock = new TransactionType()
      {
        Id = 8,
        Name = "خروج از سطح بلوکه",
        Factor = TransactionTypeFactor.Minus,
        TransactionLevel = TransactionLevel.Blocked
      };
      ImportAvailable = new TransactionType()
      {
        Id = 9,
        Name = "ورود به سطح در دسترس",
        Factor = TransactionTypeFactor.Plus,
        TransactionLevel = TransactionLevel.Available
      };
      ExportAvailable = new TransactionType()
      {
        Id = 10,
        Name = "خروج از سطح در دسترس",
        Factor = TransactionTypeFactor.Minus,
        TransactionLevel = TransactionLevel.Available
      };
      ImportConsumPlan = new TransactionType()
      {
        Id = 11,
        Name = "برنامه مصرف",
        Factor = TransactionTypeFactor.Minus,
        TransactionLevel = TransactionLevel.Plan
      };
      ImportProductionPlan = new TransactionType()
      {
        Id = 12,
        Name = "برنامه تولید",
        Factor = TransactionTypeFactor.Plus,
        TransactionLevel = TransactionLevel.Plan
      };
      ImportWastePlan = new TransactionType()
      {
        Id = 13,
        Name = "برنامه ضایعات",
        Factor = TransactionTypeFactor.Plus,
        TransactionLevel = TransactionLevel.Plan
      };
      ImportSalePlan = new TransactionType()
      {
        Id = 14,
        Name = "سفارش فروش",
        Factor = TransactionTypeFactor.Minus,
        TransactionLevel = TransactionLevel.Plan
      };
      ImportPurchaseRequest = new TransactionType()
      {
        Id = 15,
        Name = "صدور درخواست خرید",
        Factor = TransactionTypeFactor.Plus,
        TransactionLevel = TransactionLevel.Plan
      };
      ImportPurchaseOrder = new TransactionType()
      {
        Id = 16,
        Name = "صدور سفارش خرید",
        Factor = TransactionTypeFactor.Plus,
        TransactionLevel = TransactionLevel.Plan
      };
      ImportCargo = new TransactionType()
      {
        Id = 17,
        Name = "صدور محموله خرید",
        Factor = TransactionTypeFactor.Plus,
        TransactionLevel = TransactionLevel.Plan
      };
      ExportPurchaseRequest = new TransactionType()
      {
        Id = 18,
        Name = "حذف از درخواست خرید",
        Factor = TransactionTypeFactor.Minus,
        TransactionLevel = TransactionLevel.Plan
      };
      ExportPurchaseOrder = new TransactionType()
      {
        Id = 19,
        Name = "حذف از سفارش خرید",
        Factor = TransactionTypeFactor.Minus,
        TransactionLevel = TransactionLevel.Plan
      };
      ExportCargo = new TransactionType()
      {
        Id = 20,
        Name = "حذف از محموله خرید",
        Factor = TransactionTypeFactor.Minus,
        TransactionLevel = TransactionLevel.Plan
      };
      Consum = new TransactionType()
      {
        Id = 21,
        Name = "مصرف",
        Factor = TransactionTypeFactor.Minus,
        TransactionLevel = TransactionLevel.Available
      };
      Production = new TransactionType()
      {
        Id = 22,
        Name = "تولید",
        Factor = TransactionTypeFactor.Plus,
        TransactionLevel = TransactionLevel.Available
      };
      ImportWaste = new TransactionType()
      {
        Id = 23,
        Name = "ورود به ضایعات",
        Factor = TransactionTypeFactor.Plus,
        TransactionLevel = TransactionLevel.Waste
      };
      ExportWaste = new TransactionType()
      {
        Id = 24,
        Name = "خروج از ضایعات",
        Factor = TransactionTypeFactor.Minus,
        TransactionLevel = TransactionLevel.Waste
      };
      ExportBlockedFromWarehouse = new TransactionType()
      {
        Id = 25,
        Name = "خروج بلوکه از انبار",
        Factor = TransactionTypeFactor.Minus,
        TransactionLevel = TransactionLevel.Blocked
      };
      ExportAvailableFromWarehouse = new TransactionType()
      {
        Id = 26,
        Name = "خروج در دسترس از انبار",
        Factor = TransactionTypeFactor.Minus,
        TransactionLevel = TransactionLevel.Available
      };
      ImportAvailableStockAdjustment = new TransactionType()
      {
        Id = 27,
        Name = "سند اصلاحی ورود به سطح در دسترس",
        Factor = TransactionTypeFactor.Plus,
        TransactionLevel = TransactionLevel.Available
      };
      ExportAvailableStockAdjustment = new TransactionType()
      {
        Id = 28,
        Name = "سند اصلاحی خروج از سطح در دسترس",
        Factor = TransactionTypeFactor.Minus,
        TransactionLevel = TransactionLevel.Available
      };
      ImportQualityControl = new TransactionType()
      {
        Id = 29,
        Name = "ورود به سطح کنترل کیفی",
        Factor = TransactionTypeFactor.Plus,
        TransactionLevel = TransactionLevel.QualityControl
      };
      ExportQualityControl = new TransactionType()
      {
        Id = 30,
        Name = "خروج از سطح کنترل کیفی",
        Factor = TransactionTypeFactor.Minus,
        TransactionLevel = TransactionLevel.QualityControl
      };
      ConsumedQualityControl = new TransactionType()
      {
        Id = 31,
        Name = "مصرف حین کنترل کیفی",
        Factor = TransactionTypeFactor.Minus,
        TransactionLevel = TransactionLevel.QualityControl
      };
      ExportConsumPlan = new TransactionType()
      {
        Id = 32,
        Name = "خروج از برنامه مصرف",
        Factor = TransactionTypeFactor.Plus,
        TransactionLevel = TransactionLevel.Plan
      };
      ExportProductionPlan = new TransactionType()
      {
        Id = 33,
        Name = "خروج از برنامه تولید",
        Factor = TransactionTypeFactor.Minus,
        TransactionLevel = TransactionLevel.Plan
      };
      ExportWastePlan = new TransactionType()
      {
        Id = 34,
        Name = "خروج از برنامه ضایعات",
        Factor = TransactionTypeFactor.Minus,
        TransactionLevel = TransactionLevel.Plan
      };
      ExportSalePlan = new TransactionType()
      {
        Id = 35,
        Name = "خروج از سفارش فروش",
        Factor = TransactionTypeFactor.Minus,
        TransactionLevel = TransactionLevel.Plan
      };
      ImportProduction = new TransactionType()
      {
        Id = 36,
        Name = "ورود اولیه تولید",
        Factor = TransactionTypeFactor.Plus,
        TransactionLevel = TransactionLevel.Available
      };
      ImportBlockedStockAdjustment = new TransactionType()
      {
        Id = 37,
        Name = "سند اصلاحی ورود به سطح در بلوکه",
        Factor = TransactionTypeFactor.Plus,
        TransactionLevel = TransactionLevel.Blocked
      };
      ExportBlockedStockAdjustment = new TransactionType()
      {
        Id = 38,
        Name = "سند اصلاحی خروج از سطح در بلوکه",
        Factor = TransactionTypeFactor.Minus,
        TransactionLevel = TransactionLevel.Blocked
      };
      ImportQualityControlStockAdjustment = new TransactionType()
      {
        Id = 39,
        Name = "سند اصلاحی ورود به سطح در کنترل کیفی",
        Factor = TransactionTypeFactor.Plus,
        TransactionLevel = TransactionLevel.QualityControl
      };
      ExportQualityControlStockAdjustment = new TransactionType()
      {
        Id = 40,
        Name = "سند اصلاحی خروج از سطح در کنترل کیفی",
        Factor = TransactionTypeFactor.Minus,
        TransactionLevel = TransactionLevel.QualityControl
      };
      ImportWasteStockAdjustment = new TransactionType()
      {
        Id = 41,
        Name = "سند اصلاحی ورود به سطح در ضایعات",
        Factor = TransactionTypeFactor.Plus,
        TransactionLevel = TransactionLevel.Waste
      };
      ExportWasteStockAdjustment = new TransactionType()
      {
        Id = 42,
        Name = "سند اصلاحی خروج از سطح در ضایعات",
        Factor = TransactionTypeFactor.Minus,
        TransactionLevel = TransactionLevel.Waste
      };
      ImportLading = new TransactionType()
      {
        Id = 43,
        Name = "صدور بارنامه",
        Factor = TransactionTypeFactor.Plus,
        TransactionLevel = TransactionLevel.Plan
      };
      ExportLading = new TransactionType()
      {
        Id = 44,
        Name = "حذف از بارنامه",
        Factor = TransactionTypeFactor.Plus,
        TransactionLevel = TransactionLevel.Plan
      };
      ImportPartitionStuffSerialAvailable = new TransactionType()
      {
        Id = 64,
        Name = "ورود به دسترس سرشکن ",
        Factor = TransactionTypeFactor.Plus,
        TransactionLevel = TransactionLevel.Available,
        RollbackTransactionType = ExportPartitionStuffSerialAvailable
      };
      ExportPartitionStuffSerialAvailable = new TransactionType()
      {
        Id = 65,
        Name = "خروج از دسترس سرشکن ",
        Factor = TransactionTypeFactor.Minus,
        TransactionLevel = TransactionLevel.Available,
        RollbackTransactionType = ImportPartitionStuffSerialAvailable
      };
      ImportPartitionStuffSerialBlocked = new TransactionType()
      {
        Id = 66,
        Name = " ورود به بلوکه شده سرشکن",
        Factor = TransactionTypeFactor.Plus,
        TransactionLevel = TransactionLevel.Blocked,
        RollbackTransactionType = ExportPartitionStuffSerialBlocked
      };
      ExportPartitionStuffSerialBlocked = new TransactionType()
      {
        Id = 67,
        Name = "خروج از بلوکه شده سرشکن ",
        Factor = TransactionTypeFactor.Minus,
        TransactionLevel = TransactionLevel.Blocked,
        RollbackTransactionType = ImportPartitionStuffSerialBlocked
      };
      ImportPartitionStuffSerialQualityControl = new TransactionType()
      {
        Id = 68,
        Name = "ورود به کنترل کیفی سرشکن",
        Factor = TransactionTypeFactor.Plus,
        TransactionLevel = TransactionLevel.QualityControl,
        RollbackTransactionType = ExportPartitionStuffSerialQualityControl
      };
      ExportPartitionStuffSerialQualityControl = new TransactionType()
      {
        Id = 69,
        Name = "خروج از کنترل کیفی سرشکن",
        Factor = TransactionTypeFactor.Minus,
        TransactionLevel = TransactionLevel.QualityControl,
        RollbackTransactionType = ImportPartitionStuffSerialQualityControl
      };
      ImportPartitionStuffSerialWaste = new TransactionType()
      {
        Id = 70,
        Name = "ورود به  ضایعات سرشکن",
        Factor = TransactionTypeFactor.Plus,
        TransactionLevel = TransactionLevel.Waste,
        RollbackTransactionType = ExportPartitionStuffSerialWaste
      };
      ExportPartitionStuffSerialWaste = new TransactionType()
      {
        Id = 71,
        Name = "خروج از ضایعات سرشکن",
        Factor = TransactionTypeFactor.Minus,
        TransactionLevel = TransactionLevel.Waste,
        RollbackTransactionType = ImportPartitionStuffSerialWaste
      };
      TakeSample = new TransactionType()
      {
        Id = 45,
        Name = "برداشتن نمونه از سطح کنترل کیفی",
        Factor = TransactionTypeFactor.Minus,
        TransactionLevel = TransactionLevel.QualityControl
      };
      ReturnSample = new TransactionType()
      {
        Id = 46,
        Name = "بازگرداندن نمونه به سطح کنترل کیفی",
        Factor = TransactionTypeFactor.Plus,
        TransactionLevel = TransactionLevel.QualityControl
      };
      #region Fiscal Period Transactions
      CloseAvailableFiscalPeriodMinus = new TransactionType()
      {
        Id = 47,
        Name = "بستن دوره مالی سطح در دسترس - کاهش موجودی",
        Factor = TransactionTypeFactor.Minus,
        TransactionLevel = TransactionLevel.Available,
        RollbackTransactionType = ImportAvailableStockAdjustment
      };
      CloseAvailableFiscalPeriodPlus = new TransactionType()
      {
        Id = 48,
        Name = "بستن دوره مالی سطح در دسترس - افزایش موجودی",
        Factor = TransactionTypeFactor.Plus,
        TransactionLevel = TransactionLevel.Available,
        RollbackTransactionType = ExportAvailableStockAdjustment
      };
      //
      CloseBlockedFiscalPeriodMinus = new TransactionType()
      {
        Id = 49,
        Name = "بستن دوره مالی سطح بلوکه - کاهش موجودی",
        Factor = TransactionTypeFactor.Minus,
        TransactionLevel = TransactionLevel.Blocked,
        RollbackTransactionType = ImportBlockedStockAdjustment
      };
      CloseBlockedFiscalPeriodPlus = new TransactionType()
      {
        Id = 50,
        Name = "بستن دوره مالی سطح بلوکه - افزایش موجودی",
        Factor = TransactionTypeFactor.Plus,
        TransactionLevel = TransactionLevel.Blocked,
        RollbackTransactionType = ExportBlockedStockAdjustment
      };
      //
      CloseQualityControlFiscalPeriodMinus = new TransactionType()
      {
        Id = 51,
        Name = "بستن دوره مالی سطح کنترل کیفی - کاهش موجودی",
        Factor = TransactionTypeFactor.Minus,
        TransactionLevel = TransactionLevel.QualityControl,
        RollbackTransactionType = ImportQualityControlStockAdjustment
      };
      CloseQualityControlFiscalPeriodPlus = new TransactionType()
      {
        Id = 52,
        Name = "بستن دوره مالی سطح کنترل کیفی - افزایش موجودی",
        Factor = TransactionTypeFactor.Plus,
        TransactionLevel = TransactionLevel.QualityControl,
        RollbackTransactionType = ExportQualityControlStockAdjustment
      };
      //
      CloseWasteFiscalPeriodMinus = new TransactionType()
      {
        Id = 53,
        Name = "بستن دوره مالی سطح مردودی - کاهش موجودی",
        Factor = TransactionTypeFactor.Minus,
        TransactionLevel = TransactionLevel.Waste,
        RollbackTransactionType = ImportWasteStockAdjustment
      };
      CloseWasteFiscalPeriodPlus = new TransactionType()
      {
        Id = 54,
        Name = "بستن دوره مالی سطح مردودی - افزایش موجودی",
        Factor = TransactionTypeFactor.Plus,
        TransactionLevel = TransactionLevel.Waste,
        RollbackTransactionType = ExportWasteStockAdjustment
      };
      //
      OpenAvailableFiscalPeriodMinus = new TransactionType()
      {
        Id = 55,
        Name = "باز کردن دوره مالی سطح در دسترس - کاهش موجودی",
        Factor = TransactionTypeFactor.Minus,
        TransactionLevel = TransactionLevel.Available,
        RollbackTransactionType = ImportAvailableStockAdjustment
      };
      OpenAvailableFiscalPeriodPlus = new TransactionType()
      {
        Id = 56,
        Name = "باز کردن دوره مالی سطح در دسترس - افزایش موجودی",
        Factor = TransactionTypeFactor.Plus,
        TransactionLevel = TransactionLevel.Available,
        RollbackTransactionType = ExportAvailableStockAdjustment
      };
      //
      OpenBlockedFiscalPeriodMinus = new TransactionType()
      {
        Id = 57,
        Name = "باز کردن دوره مالی سطح بلوکه - کاهش موجودی",
        Factor = TransactionTypeFactor.Minus,
        TransactionLevel = TransactionLevel.Blocked,
        RollbackTransactionType = ImportBlockedStockAdjustment
      };
      OpenBlockedFiscalPeriodPlus = new TransactionType()
      {
        Id = 58,
        Name = "باز کردن دوره مالی سطح بلوکه - افزایش موجودی",
        Factor = TransactionTypeFactor.Plus,
        TransactionLevel = TransactionLevel.Blocked,
        RollbackTransactionType = ExportBlockedStockAdjustment
      };
      //
      OpenQualityControlFiscalPeriodMinus = new TransactionType()
      {
        Id = 59,
        Name = "باز کردن دوره مالی سطح کنترل کیفی - کاهش موجودی",
        Factor = TransactionTypeFactor.Minus,
        TransactionLevel = TransactionLevel.QualityControl,
        RollbackTransactionType = ImportQualityControlStockAdjustment
      };
      OpenQualityControlFiscalPeriodPlus = new TransactionType()
      {
        Id = 60,
        Name = "باز کردن دوره مالی سطح کنترل کیفی - افزایش موجودی",
        Factor = TransactionTypeFactor.Plus,
        TransactionLevel = TransactionLevel.QualityControl,
        RollbackTransactionType = ExportQualityControlStockAdjustment
      };
      //
      OpenWasteFiscalPeriodMinus = new TransactionType()
      {
        Id = 61,
        Name = "باز کردن دوره مالی سطح مردودی - کاهش موجودی",
        Factor = TransactionTypeFactor.Minus,
        TransactionLevel = TransactionLevel.Waste,
        RollbackTransactionType = ImportWasteStockAdjustment
      };
      OpenWasteFiscalPeriodPlus = new TransactionType()
      {
        Id = 62,
        Name = "باز کردن دوره مالی سطح مردودی - افزایش موجودی",
        Factor = TransactionTypeFactor.Plus,
        TransactionLevel = TransactionLevel.Waste,
        RollbackTransactionType = ExportWasteStockAdjustment
      };
      //
      #endregion
      ReturnToLading = new TransactionType()
      {
        Id = 63,
        Name = "بازگرداندن بارنامه",
        Factor = TransactionTypeFactor.Plus,
        TransactionLevel = TransactionLevel.Plan
      };
      ExportAsDirectStoreIssue.RollbackTransactionType = ImportAvailable;
      ImportAsDirectStoreIssue.RollbackTransactionType = ExportAvailable;
      ExportAsIndirectStoreIssue.RollbackTransactionType = ImportAvailable;
      ImportAsBlockedStoreIssue.RollbackTransactionType = ExportBlock;
      ExportAsConfirmedBlockedStoreIssue.RollbackTransactionType = ImportBlock;
      ExportAsRejectedBlockedStoreIssue.RollbackTransactionType = ImportBlock;
      ImportAsIndirectStoreIssue.RollbackTransactionType = ExportAvailable;
      ImportBlock.RollbackTransactionType = ExportBlock;
      ExportBlock.RollbackTransactionType = ImportBlock;
      ImportAvailable.RollbackTransactionType = ExportAvailable;
      ExportAvailable.RollbackTransactionType = ImportAvailable;
      Production.RollbackTransactionType = ImportAvailable;
      ExportWaste.RollbackTransactionType = ImportWaste;
      ImportQualityControl.RollbackTransactionType = ExportQualityControl;
      ExportQualityControl.RollbackTransactionType = ImportQualityControl;
      ConsumedQualityControl.RollbackTransactionType = ImportQualityControl;
      ImportConsumPlan.RollbackTransactionType = ExportConsumPlan;
      ExportConsumPlan.RollbackTransactionType = ImportConsumPlan;
      ImportProductionPlan.RollbackTransactionType = ExportProductionPlan;
      ExportProductionPlan.RollbackTransactionType = ImportProductionPlan;
      ImportWastePlan.RollbackTransactionType = ExportWastePlan;
      ExportWastePlan.RollbackTransactionType = ImportWastePlan;
      ImportSalePlan.RollbackTransactionType = ExportSalePlan;
      ExportSalePlan.RollbackTransactionType = ImportSalePlan;
      ImportBlockedStockAdjustment.RollbackTransactionType = ExportBlockedStockAdjustment;
      ExportBlockedStockAdjustment.RollbackTransactionType = ImportBlockedStockAdjustment;
      ImportQualityControlStockAdjustment.RollbackTransactionType = ExportQualityControlStockAdjustment;
      ExportQualityControlStockAdjustment.RollbackTransactionType = ImportQualityControlStockAdjustment;
      ImportWasteStockAdjustment.RollbackTransactionType = ExportWasteStockAdjustment;
      ExportWasteStockAdjustment.RollbackTransactionType = ImportWasteStockAdjustment;
      ImportAvailableStockAdjustment.RollbackTransactionType = ExportAvailableStockAdjustment;
      ExportAvailableStockAdjustment.RollbackTransactionType = ImportAvailableStockAdjustment;
      ReturnToLading.RollbackTransactionType = ExportLading;
    }
    public static lena.Domains.TransactionType ExportAsDirectStoreIssue { get; }
    public static lena.Domains.TransactionType ImportAsDirectStoreIssue { get; }
    public static lena.Domains.TransactionType ExportAsIndirectStoreIssue { get; }
    public static lena.Domains.TransactionType ImportAsBlockedStoreIssue { get; }
    public static lena.Domains.TransactionType ExportAsConfirmedBlockedStoreIssue { get; }
    public static lena.Domains.TransactionType ExportAsRejectedBlockedStoreIssue { get; }
    public static lena.Domains.TransactionType ImportAsIndirectStoreIssue { get; }
    public static lena.Domains.TransactionType ImportProduction { get; }
    public static lena.Domains.TransactionType ImportBlock { get; }
    public static lena.Domains.TransactionType ExportBlock { get; }
    public static lena.Domains.TransactionType ImportAvailable { get; }
    public static lena.Domains.TransactionType ExportAvailable { get; }
    public static lena.Domains.TransactionType ImportConsumPlan { get; }
    public static lena.Domains.TransactionType ImportProductionPlan { get; }
    public static lena.Domains.TransactionType ImportWastePlan { get; }
    public static lena.Domains.TransactionType ImportSalePlan { get; }
    public static lena.Domains.TransactionType ImportPurchaseRequest { get; }
    public static lena.Domains.TransactionType ImportPurchaseOrder { get; }
    public static lena.Domains.TransactionType ImportCargo { get; }
    public static lena.Domains.TransactionType ImportLading { get; }
    public static lena.Domains.TransactionType ExportLading { get; }
    public static lena.Domains.TransactionType ImportPartitionStuffSerialAvailable { get; }
    public static lena.Domains.TransactionType ExportPartitionStuffSerialAvailable { get; }
    public static lena.Domains.TransactionType ImportPartitionStuffSerialBlocked { get; }
    public static lena.Domains.TransactionType ExportPartitionStuffSerialBlocked { get; }
    public static lena.Domains.TransactionType ImportPartitionStuffSerialQualityControl { get; }
    public static lena.Domains.TransactionType ExportPartitionStuffSerialQualityControl { get; }
    public static lena.Domains.TransactionType ImportPartitionStuffSerialWaste { get; }
    public static lena.Domains.TransactionType ExportPartitionStuffSerialWaste { get; }
    public static lena.Domains.TransactionType ExportPurchaseRequest { get; }
    public static lena.Domains.TransactionType ExportPurchaseOrder { get; }
    public static lena.Domains.TransactionType ExportCargo { get; }
    public static lena.Domains.TransactionType Consum { get; }
    public static lena.Domains.TransactionType Production { get; }
    public static lena.Domains.TransactionType ImportWaste { get; }
    public static lena.Domains.TransactionType ExportWaste { get; }
    public static lena.Domains.TransactionType ExportBlockedFromWarehouse { get; }
    public static lena.Domains.TransactionType ExportAvailableFromWarehouse { get; }
    public static lena.Domains.TransactionType ImportAvailableStockAdjustment { get; }
    public static lena.Domains.TransactionType ExportAvailableStockAdjustment { get; }
    public static lena.Domains.TransactionType ImportQualityControl { get; }
    public static lena.Domains.TransactionType ExportQualityControl { get; }
    public static lena.Domains.TransactionType ConsumedQualityControl { get; }
    public static lena.Domains.TransactionType ExportConsumPlan { get; }
    public static lena.Domains.TransactionType ExportProductionPlan { get; }
    public static lena.Domains.TransactionType ExportWastePlan { get; }
    public static lena.Domains.TransactionType ExportSalePlan { get; }
    public static lena.Domains.TransactionType ImportBlockedStockAdjustment { get; }
    public static lena.Domains.TransactionType ExportBlockedStockAdjustment { get; }
    public static lena.Domains.TransactionType ImportQualityControlStockAdjustment { get; }
    public static lena.Domains.TransactionType ExportQualityControlStockAdjustment { get; }
    public static lena.Domains.TransactionType ImportWasteStockAdjustment { get; }
    public static lena.Domains.TransactionType ExportWasteStockAdjustment { get; }
    public static lena.Domains.TransactionType TakeSample { get; }
    public static lena.Domains.TransactionType ReturnSample { get; }
    public static lena.Domains.TransactionType CloseAvailableFiscalPeriodMinus { get; }
    public static lena.Domains.TransactionType CloseAvailableFiscalPeriodPlus { get; }
    public static lena.Domains.TransactionType CloseBlockedFiscalPeriodMinus { get; }
    public static lena.Domains.TransactionType CloseBlockedFiscalPeriodPlus { get; }
    public static lena.Domains.TransactionType CloseQualityControlFiscalPeriodMinus { get; }
    public static lena.Domains.TransactionType CloseQualityControlFiscalPeriodPlus { get; }
    public static lena.Domains.TransactionType CloseWasteFiscalPeriodMinus { get; }
    public static lena.Domains.TransactionType CloseWasteFiscalPeriodPlus { get; }
    public static lena.Domains.TransactionType OpenAvailableFiscalPeriodMinus { get; }
    public static lena.Domains.TransactionType OpenAvailableFiscalPeriodPlus { get; }
    public static lena.Domains.TransactionType OpenBlockedFiscalPeriodMinus { get; }
    public static lena.Domains.TransactionType OpenBlockedFiscalPeriodPlus { get; }
    public static lena.Domains.TransactionType OpenQualityControlFiscalPeriodMinus { get; }
    public static lena.Domains.TransactionType OpenQualityControlFiscalPeriodPlus { get; }
    public static lena.Domains.TransactionType OpenWasteFiscalPeriodMinus { get; }
    public static lena.Domains.TransactionType OpenWasteFiscalPeriodPlus { get; }
    public static lena.Domains.TransactionType ReturnToLading { get; set; }
  }
}