using System.Collections.Generic;

using lena.Domains.Enums;
namespace lena.Models.Production.ProductionLineEmployeeInterval
{
  public class EmployeeOperationList
  {
    public int EmployeeId { get; set; }

    public IEnumerable<OperationList> OperationTimes { get; set; }
  }
}
