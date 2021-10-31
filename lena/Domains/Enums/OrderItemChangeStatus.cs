using lena.Domains.Enums;
namespace lena.Domains.Enums
{
  public enum OrderItemChangeStatus : byte
  {
    NotAction,
    SaleConfirmed,
    SaleRejected,
    PlanConfirmed,
    PlanRejected,
    DirectConfirmed,
    InitialInsertion
  }
}
