using lena.Domains.Enums;
namespace lena.Models.ProjectManagement.ProjectERP
{
  public class ProjectERPComboResult
  {
    public string Title { get; set; }
    public string Code { get; set; }
    public int Version { get; set; }
    public bool IsActive { get; set; }
    public int? StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public int? CustomerId { get; set; }
    public string CustomerName { get; set; }
  }
}
