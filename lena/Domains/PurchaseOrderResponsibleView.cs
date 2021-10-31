using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public class PurchaseOrderResponsibleView : IEntity
  {
    public int PurchaseOrderId { get; set; }
    public string ResponsibleFullNames { get; set; }
    public byte[] RowVersion { get; set; }
  }
}