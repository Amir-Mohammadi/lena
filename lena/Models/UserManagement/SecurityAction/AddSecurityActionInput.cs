using lena.Domains.Enums;
namespace lena.Models.UserManagement.SecurityAction
{
  public class AddSecurityActionInput
  {
    public string Name { get; set; }
    public string ActionName { get; set; }
    public int SecurityActionGroupId { get; set; }
    public AddActionParameterInput[] AddActionParameters { get; set; }
  }


}
