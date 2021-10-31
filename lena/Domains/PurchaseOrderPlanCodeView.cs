using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public class PurchaseOrderPlanCodeView : IEntity
  {
    public int PurchaseOrderId { get; set; }
    public string PlanCodes { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
