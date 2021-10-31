using lena.Domains.Enums;
using lena.Models.WarehouseManagement.StuffRequestItem;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StuffRequest
{
  public class EditStuffRequestInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
    public int FromWarehouseId { get; set; }
    public int? ToWarehouseId { get; set; }
    public int? EmployeeId { get; set; }
    public int? DepartmentId { get; set; }
    public int? ScrumProjectId { get; set; }
    public StuffRequestType StuffRequestType { get; set; }
    public AddStuffRequestItemInput[] AddStuffRequestItems { get; set; }
    public EditStuffRequestItemInput[] EditStuffRequestItems { get; set; }
    public DeleteStuffRequestItemInput[] DeleteStuffRequestItems { get; set; }
    public string Description { get; set; }
  }
}
