using System;

using lena.Domains.Enums;
namespace lena.Models.SaleManagement.PaymentType
{
  public class PaymentTypeResult
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
    public string Name { get; set; }
    public Boolean IsActive { get; set; }
  }
}
