using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.TagCounting
{
  public class DeleteTagCountingInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
