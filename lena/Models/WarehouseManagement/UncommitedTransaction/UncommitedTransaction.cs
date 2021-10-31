using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.UncommitedTransaction
{
  public class UncommitedTransaction
  {

    public int Id { get; set; }

    public int TransactionBatchId { get; set; }

    public short TransactionTypeId { get; set; }
    public int StuffId { get; set; }
    public long? StuffSerialCode { get; set; }
    public double Amount { get; set; }
    public byte UnitId { get; set; }
    public short? WarehouseId { get; set; }

    public TransactionLevel TransactionLevel { get; set; }

    public string Description { get; set; }
    public string Serial { get; set; }

    public string WarehouseName { get; set; }

    public int Factor { get; set; }
  }
}
