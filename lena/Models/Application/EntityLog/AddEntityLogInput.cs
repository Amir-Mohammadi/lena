using lena.Domains.Enums;
namespace lena.Models.Application.EntityLog
{
  public class AddEntityLogInput
  {
    public string EntityType { get; set; }
    public string ApiParams { get; set; }
    public string Api { get; set; }
    public string Description { get; set; }
  }
}
