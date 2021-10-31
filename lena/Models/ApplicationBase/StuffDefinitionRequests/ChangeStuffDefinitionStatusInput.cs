using lena.Domains.Enums;
namespace lena.Models.ApplicationBase.StuffDefinitionRequests
{
  public class ChangeStuffDefinitionStatusInput
  {
    public int Id { get; set; }
    public bool IsConfirmed { get; set; }
    public string ConfirmDescription { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
