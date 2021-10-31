using lena.Domains.Enums;
using System.Linq;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.Warehouse
{
  public class WarehouseResult
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public bool IsDelete { get; set; }
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; }
    public bool FIFO { get; set; }
    public int? DisplayOrder { get; set; }
    public WarehouseType WarehouseType { get; set; }
    public IQueryable<StoreReceiptType> StoreReceiptTypes { get; set; }
    public IQueryable<TransactionLevel> TransactionLevels { get; set; }
    public IQueryable<StuffType> StuffTypes { get; set; }
    public IQueryable<int> ExitReciptRequestType { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
