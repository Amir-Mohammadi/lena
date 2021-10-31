using System.Linq;
using lena.Models.Production.StuffProductionFaultType;

using lena.Domains.Enums;
namespace lena.Models.Production.ProductionFaultType
{
  public class ProductionFaultTypeFullResult
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public int? OperationId { get; set; }
    public string OperationTitle { get; set; }
    public IQueryable<StuffProductionFaultTypeResult> StuffProductionFaultTypes { get; set; }
    public byte[] RowVersion { get; set; }

  }
}
