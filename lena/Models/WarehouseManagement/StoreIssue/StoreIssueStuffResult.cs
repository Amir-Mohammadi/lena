using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StoreIssue
{
  public class StoreIssueStuffResult
  {
    public int Id { get; set; }

    public int StuffId { get; set; }
    public long? StuffSerialCode { get; set; }

    public string StuffName { get; set; }

    public double Amount { get; set; }

    public byte UnitId { get; set; }

    public string UnitName { get; set; }

    public byte[] RowVersion { get; set; }

  }
}
