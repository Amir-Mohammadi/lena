using System;
using System.Collections.Generic;
using lena.Models.Supplies.ProvisionersCartItem;


using lena.Domains.Enums;
namespace lena.Models.Supplies.ProvisionersCart
{
  public class FullProvisionersCartResult
  {
    public int Id { get; set; }
    public string Description { get; set; }
    public Nullable<int> ResponsibleEmployeeId { get; set; }
    public string RegisterEmployeeFullName { get; set; }
    public Nullable<System.DateTime> RegisterDateTime { get; set; }
    public Nullable<System.DateTime> ReportDate { get; set; }
    public Nullable<lena.Domains.Enums.ProvisionersCartStatus> Status { get; set; }
    public Nullable<int> SupplierId { get; set; }
    public byte[] RowVersion { get; set; }
    public string SupplierName { get; set; }
    public IEnumerable<ProvisionersCartItemResult> ProvisionersCartItems { get; set; }
  }
}