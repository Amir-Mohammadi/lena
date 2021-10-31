using System;

using lena.Domains.Enums;
namespace lena.Domains.Enums
{
  [Flags]
  public enum OrderItemStatus
  {
    None = 0,
    Order = 1,
    SaleConfirmed = 2,
    SaleRejected = 4,
    PlanningConfirmed = 8,
    PlanningRejected = 16,
    Planning = 32,
    InProduction = 64,
    Blocking = 128,
    Blocked = 256,
    Sending = 512,
    Sent = 1024,
    Completed = 2048,
    Deactive = 4096,
  }
}
