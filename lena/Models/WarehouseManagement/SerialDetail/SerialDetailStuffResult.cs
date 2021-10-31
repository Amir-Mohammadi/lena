using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.SerialDetail
{
  public class SerialDetailStuffResult
  {
    public string LinkedSerial { get; set; }
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public string StuffNoun { get; set; }
    public int Count { get; set; }
  }
}