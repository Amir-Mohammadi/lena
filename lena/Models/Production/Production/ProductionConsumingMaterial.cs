using System.Linq;
using lena.Domains.Enums;
using lena.Models.WarehouseManagement.SerialBuffer;

using lena.Domains.Enums;
namespace lena.Models.Production.Production
{
  public class ProductionConsumingMaterial
  {
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public BillOfMaterialDetailType BillOfMaterialDetailType { get; set; }
    public short? BillOfMaterialVersion { get; set; }
    public double Qty { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public double SerialBufferRemainingAmount { get; set; }
    public double WarehouseRemainingAmount { get; set; }
    public IQueryable<SerialBufferMinResult> SerialBuffers { get; set; }
    public bool IsEquvalent { get; set; }
    public bool HasEquvalent { get; set; }
    public bool LimitedSerialBuffer { get; set; }
    public double SerialBufferShortageAmount { get; set; }
    public double SerialBufferDamagedAmount { get; set; }
  }
}
