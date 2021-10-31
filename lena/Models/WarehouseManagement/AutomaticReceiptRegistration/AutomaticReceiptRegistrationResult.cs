using lena.Domains.Enums;
using System;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.AutomaticReceiptRegistration
{
  public class AutomaticReceiptRegistrationResult
  {
    public int LadingId { get; set; }

    public string LadingCode { get; set; }

    public DateTime LadingDateTime { get; set; }

    public int ProviderId { get; set; }

    public string ProviderName { get; set; }

    public DateTime EntranceDateTime { get; set; }

    public ProviderType ProviderType { get; set; }


  }
}
