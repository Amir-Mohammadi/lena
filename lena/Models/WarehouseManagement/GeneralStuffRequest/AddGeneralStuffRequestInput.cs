using lena.Domains.Enums;
using System;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.GeneralStuffRequest
{
  public class AddGeneralStuffRequestInput
  {
    public int? ProductionMaterialRequestId { get; set; }
    public StuffRequestType StuffRequestType { get; set; }
    public int StuffId { get; set; }
    public byte UnitId { get; set; }
    public short? BillOfMaterialVersion { get; set; }
    public double Qty { get; set; }
    public DateTime Deadline { get; set; }
    public int? ScrumEntityId { get; set; }
    public short FromWarehouseId { get; set; }
    public short? ToWarehouseId { get; set; }
    public short? ToDepartmentId { get; set; }
    public int? ToEmployeeId { get; set; }
    public string Description { get; set; }
  }
}
