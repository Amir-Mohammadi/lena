using lena.Domains.Enums;
namespace lena.Models.ApplicationBase.StuffDefinitionRequests
{
  public class EditStuffDefinitionRequestInput : AddStuffDefinitionRequestInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
