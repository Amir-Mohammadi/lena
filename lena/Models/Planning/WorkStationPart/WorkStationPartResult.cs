using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Planning.WorkStationPart
{
  public class WorkStationPartResult
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int ProductionLineId { get; set; }
    public string ProductionLineName { get; set; }
    public int WorkStationId { get; set; }
    public string WorkStationName { get; set; }
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; }
    public WorkStationPartType WorkStationPartType { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
