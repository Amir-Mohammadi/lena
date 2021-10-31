using lena.Models.WarehouseManagement.PreparingSendingItem;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.PreparingSending
{
  public class AddPreparingSendingInput
  {

    public int SendPermissionId { get; set; }
    public AddPreparingSendingItemInput[] PreparingSendingItems { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
