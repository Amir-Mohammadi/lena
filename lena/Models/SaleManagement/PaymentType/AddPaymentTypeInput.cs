
using System;

using lena.Domains.Enums;
namespace lena.Models.SaleManagement.PaymentType
{
  public class AddPaymentTypeInput
  {
    public string Name { get; set; }

    public Boolean IsActive { get; set; }
  }
}
