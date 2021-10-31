using lena.Domains.Enums;
using lena.Models.WarehouseManagement.StuffRequestItem;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StuffRequest
{
  public class AddStuffRequestInput
  {
    public short FromWarehouseId { get; set; }
    public short? ToWarehouseId { get; set; }
    public int? EmployeeId { get; set; }
    public short? DepartmentId { get; set; }
    public StuffRequestType StuffRequestType { get; set; }
    public AddStuffRequestItemInput[] StuffRequestItems { get; set; }
    public int? ScrumProjectId { get; set; }
    public string Description { get; set; }
  }
}
