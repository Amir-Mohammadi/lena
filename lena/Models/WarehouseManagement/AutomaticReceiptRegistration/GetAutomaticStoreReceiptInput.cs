using System;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.AutomaticReceiptRegistration
{
  public class GetAutomaticStoreReceiptInput
  {
    public int LadingId { get; set; }

    public int ProviderId { get; set; }

    public DateTime EntranceDateTime { get; set; }
  }
}
