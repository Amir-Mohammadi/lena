using lena.Domains.Enums;
namespace lena.Models.Planning.WorkStation
{
  public class EditWorkStationInput
  {
    public short Id { get; set; }
    public byte[] RowVersion { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int ProductionLineId { get; set; }
  }
}
