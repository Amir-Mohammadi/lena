using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.Transaction
{
  public class WarehouseIssueInput
  {
    public int SourceWarehouseId { get; set; }
    public int DestinationWarehouseId { get; set; }

    public Item[] Items { get; set; }

    public class Item
    {
      public int StuffId { get; set; }
      public long StuffSerialCode { get; set; }
      public double Amount { get; set; }
      public byte UnitId { get; set; }
    }
  }
}
