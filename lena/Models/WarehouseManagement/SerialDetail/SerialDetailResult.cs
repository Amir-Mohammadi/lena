using lena.Domains.Enums;
using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.SerialDetail
{
  public class SerialDetailResult
  {
    public string LinkedSerial { get; set; }
    public string TechnicalNumber { get; set; }
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public string Serial { get; set; }
    public double Qty { get; set; }
    public int Version { get; set; }
    public int WorkPlanStepId { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public StuffSerialStatus? Status { get; set; }
    public byte[] RowVersion { get; set; }
  }
}