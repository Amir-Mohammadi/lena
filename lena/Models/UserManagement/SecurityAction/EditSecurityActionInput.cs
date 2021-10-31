using lena.Domains.Enums;
namespace lena.Models.UserManagement.SecurityAction
{
  public class EditSecurityActionInput
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string ActionName { get; set; }
    public int SecurityActionGroupId { get; set; }
    public AddActionParameterInput[] AddActionParameters { get; set; }
    public EditActionParameterInput[] EditActionParameters { get; set; }
    public int[] DeleteActionParameters { get; set; }
    public byte[] RowVersion { get; set; }
  }
}