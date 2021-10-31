using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models
{
  public class ContactResult
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Title { get; set; }
    public bool IsMain { get; set; }
    public string ContactText { get; set; }
    public int ContactTypeId { get; set; }
    public string ContactTypeName { get; set; }
    public CooperatorContactType CooperatorContactType { get; set; }
    public bool IsEmployeeContact { get; set; }
    public bool IsCustomerContact { get; set; }
    public bool IsProviderContact { get; set; }
  }
}
