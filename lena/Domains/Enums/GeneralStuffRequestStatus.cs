using lena.Domains.Enums;
namespace lena.Domains.Enums
{
  public enum GeneralStuffRequestStatus : short
  {
    NotAction = 1,
    Confirmed = 2,
    Rejected = 4,
    AlternativePurchaseRequest = 8,
    PurchaseRequest = 16,
    WarehouseRequest = 32
  }

}
