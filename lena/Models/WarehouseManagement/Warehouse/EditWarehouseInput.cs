using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.Warehouse
{
  public class EditWarehouseInput
  {
    public short Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public bool FIFO { get; set; }
    public short DepartmentId { get; set; }
    public int? DisplayOrder { get; set; }
    public WarehouseType WarehouseType { get; set; }

    public StoreReceiptType[] StoreReceiptTypes { get; set; }
    public int[] ExitReceiptTypeIds { get; set; }
    public TransactionLevel[] TransactionLevels { get; set; }
    public StuffType[] StuffTypes { get; set; }

    public StoreReceiptType[] DeleteStoreReceiptTypes { get; set; }
    public int[] DeleteExitReceiptTypeIds { get; set; }
    public TransactionLevel[] DeleteTransactionLevels { get; set; }
    public StuffType[] DeleteStuffTypes { get; set; }

    public byte[] RowVersion { get; set; }
  }
}
