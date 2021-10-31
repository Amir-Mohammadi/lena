using System.Linq;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.WarehouseReports
{
  public class WarehouseEnterExitStuff
  {
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public double? TotalEnterQty { get; set; }
    public double? TotalExitQty { get; set; }
    public IQueryable<WarehouseEnterExitSerial> Serials { get; set; }

  }


}
