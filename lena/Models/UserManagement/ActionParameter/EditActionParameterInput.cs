using lena.Domains.Enums;
namespace lena.Models.UserManagement.SecurityAction
{
  public class EditActionParameterInput
  {
    public int Id { get; set; }
    public string Key { get; set; }
    public string Value { get; set; }
    public bool CheckParameterValue { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
