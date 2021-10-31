using lena.Domains.Enums;
namespace lena.Models
{
  public class CheckParametersAPIInput
  {
    public string Url { get; set; }
    public string Param { get; set; }
    public string SortTypeName { get; set; }
    public string SortTypeFieldName { get; set; }
  }
}
