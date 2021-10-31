using lena.Domains.Enums;
namespace lena.Domains.Enums
{
  public enum SettingKey : short
  {
    UserMaxFailedLoginCount = 0,
    UserLockOutTime = 1,
    BarcodeOneColumnReport = 2,
    BarcodeThreeColumnReport = 3,
    CompanyId = 4,
    NumberOfPricesForAveraging = 5,
    MaxPublishedBomCount = 6,
    SerialBatchCount = 7,
    MinimumSerialBufferAmount = 8,
    ShowNotification = 9,
    /// <summary>
    /// Barcode Footer Text (Company Info)
    /// </summary>
    BarcodeLabelFooterText = 10,
    CheckTransactionBatchNegativeStuffValues = 11,
    CheckTransactionBatchNegativeStuffSerialValues = 12,
    CheckTransactionBatchFragmentedSerials = 13,
    CheckProductionPlanningWhenDeactivateBom = 14,
    PasswordValidity = 15,
    WarehouseIssueConfirmDeadline = 16,
    HashedVersion = 17,
    ApplicationLogEnabled = 18,
    RedisHost = 19,
    RedisPort = 20,
    TokenTimeout = 21,
    Issuer = 22,
    SecretKey = 23,
    Encryptkey = 24,
    CheckInternalProvider = 25,
    CheckForeignProvider = 26,
    NormalBoardTime = 27,
    DurationOfQualityControlReview = 28,
    NotificationHost = 29,
    NotificationPort = 30,
    LoggerHost = 31,
    LoggerPort = 32,
    LoggerToken = 33,
    ICLIW = 34, //IntervalCargoItemAndLadingItemIndicatorWeight شناسه وزن های مربوط به فاصله زمانی محموله تا بارنامه
    ILNIW = 35, //IntervalLadingItemAndNewShoppingIndicatorWeight شناسه وزن های مربوط به فاصله زمانی بارنامه تا تحویل به شرکت
    QCIOD = 36, // شناسه وزن های مربوط به کنترل کیفی به موقع
    QCILD = 37, // شناسه وزن های مربوط به عدم کنترل کیفی به موقع
    ThresholdDate = 39,
    AllMembersGroup = 40,
    TerminatedBanOrder = 41,
    StoreReceiptDaysAfterInboundCargo = 42, // تعداد روزهایی که امکان ثبت سند ورود کالا پس از ثبت تردد وجود دارد
    LocalIpArddressPaterns = 43,
    TimeZoneOffsetTolerance = 44, // تولرانس منطقه زمانی به دقیقه
    PurchaseRequestEssentialDateTime = 45, // مدت زمان مشخص کننده ضروری بودن درخواست خرید 
    MaximumReprintAmount = 46 // حداکثر تعداد سریال ها هنگام چاپ مجدد با فیلتر گام 
  }
}
