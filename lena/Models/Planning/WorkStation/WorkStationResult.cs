using lena.Domains.Enums;
namespace lena.Models.Planning.WorkStation
{
  public class WorkStationResult
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int ProductionLineId { get; set; }
    public string ProductionLineName { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
