using lena.Domains.Enums;
namespace lena.Domains.Enums
{
  public enum ReturnOfSaleStatus : byte
  {
    Waiting,
    WarrantyExpired,
    ForecedToRepair,
    Repairing,
    Unrepairable,
    ReworkRequired,
    ReworkNotRequired,
    PreparingForDelivery,
    Cash,
    Replacement,
    Delivered,


  }
}
