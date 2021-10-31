using lena.Domains.Enums;
namespace lena.Models
{
  public class AddApiInfoInput
  {
    public string Url { get; set; }
    public string Name { get; set; }
    public string Param { get; set; }
    public string Description { get; set; }
    public string SortTypeName { get; set; }
    public string SortTypeFeildName { get; set; }
  }
}
