using System.Linq;
using lena.Models.Production.ProductionStuffDetail;

using lena.Domains.Enums;
namespace lena.Models.Production.RepairProductionFault
{
  public class RepairProductionFaultResult
  {
    public int Id { get; set; }
    public int RepairProductionId { get; set; }
    public string ProductionFaultTypeTitle { get; set; }
    public int ProductionFaultTypeId { get; set; }
    public IQueryable<ProductionStuffDetailResult> RepairProductionStuffDetails { get; set; }
    public byte[] RowVersion { get; set; }
  }
}