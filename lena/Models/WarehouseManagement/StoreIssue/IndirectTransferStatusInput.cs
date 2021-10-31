using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.Transaction
{
  public class IndirectTransferStatusInput
  {
    public byte[] RowVersion { get; set; }

    public int StoreIssueId { get; set; }
  }
}
