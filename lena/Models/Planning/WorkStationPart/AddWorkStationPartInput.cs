using lena.Domains.Enums;
namespace lena.Models.Planning.WorkStationPart
{
  public class AddWorkStationPartInput
  {
    public string Name { get; set; }
    public short WorkStationId { get; set; }
    public string Description { get; set; }
  }
}
