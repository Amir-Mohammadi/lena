using lena.Domains.Enums;
namespace lena.Models.QualityControl.ReworkProductionOperationReport
{
  public class ProductionOperationQueryResult
  {
    public int Id { get; set; }
    public bool IsFaultCause { get; set; }
    public int? EmployeeId { get; set; }
    public int OperationId { get; set; }
    public int? FaildProductionOperationId { get; set; }
    public int? ReworkProductionOperationId { get; set; }
    public int? ProductionOperationEmployeeGroupId { get; set; }
  }
}
