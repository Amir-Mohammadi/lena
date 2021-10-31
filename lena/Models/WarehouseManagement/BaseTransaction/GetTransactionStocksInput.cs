using System;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.BaseTransaction
{
  public class GetTransactionStocksInput
  {
    public StuffInfoInput[] StuffInfos { get; set; }
    public DateTime FromDateTime { get; set; }
    public DateTime ToDateTime { get; set; }
  }
}
