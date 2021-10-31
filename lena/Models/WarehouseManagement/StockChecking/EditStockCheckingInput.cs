using lena.Models.WarehouseManagement.StockChecking;

using lena.Domains.Enums;
namespace lena.Models
{
  public class EditStockCheckingInput : AddStockCheckingInput
  {
    public int Id { get; set; }
    public int? ActiveTagTypeId { get; set; }
    public int[] AddedUsers { get; set; }
    public int[] DeletedUsers { get; set; }
    public short[] AddedWarehouses { get; set; }
    public short[] DeletedWarehouses { get; set; }
    public int[] AddedStuffs { get; set; }
    public int[] DeletedStuffs { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
