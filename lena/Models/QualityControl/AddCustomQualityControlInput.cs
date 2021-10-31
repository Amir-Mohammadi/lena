using lena.Models.QualityControl.QualityControlItem;

using lena.Domains.Enums;
namespace lena.Models.QualityControl
{
  public class AddCustomQualityControlInput
  {
    public int StuffId { get; set; }
    public short WarehouseId { get; set; }
    public string Description { get; set; }
    public AddQualityControlItemTransactionInput[] QualityControlItemTransactions { get; set; }
  }
}
