using lena.Domains.Enums;
using System.ComponentModel.DataAnnotations;

using lena.Domains.Enums;
namespace lena.Models
{
  public class AddWarehouseInput
  {
    [Required]
    [MaxLength(512)]
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

  }
}
