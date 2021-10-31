using lena.Domains.Enums;
namespace lena.Models.Planning.WorkStationPart
{
  public class EditWorkStationPartInput
  {
    public byte[] RowVersion { get; set; }
    public int Id { get; set; }
    public string Description { get; set; }
    public string Name { get; set; }
    public short WorkStationId { get; set; }
  }
}
