using System.Collections.Generic;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement
{
  public class SendSerialInput
  {
    public int FromWarehouse { get; set; }
    public int ToWareHouse { get; set; }

    public List<SendSerialItemInput> TransactionInfo { get; set; }
  }
}