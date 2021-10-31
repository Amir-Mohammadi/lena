using lena.Domains.Enums;
namespace lena.Models.QualityControl.SerialFailedOperation
{
  public class SerialFailedProductionInfoResult
  {
    public int OperationId { get; set; }
    public string OperationTitle { get; set; }
    public int EmployeeId { get; set; }
    public string EmployeeFullName { get; set; }
    public int ProductionOperationEmployeeId { get; set; }
    public int? SerialFailedOperationFaultOperationId { get; set; }
  }
}
