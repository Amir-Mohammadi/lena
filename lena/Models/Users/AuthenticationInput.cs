using System.ComponentModel.DataAnnotations;
using lena.Domains.Enums;
namespace lena.Models.Users
{
  public class AuthenticationInput
  {
    [Phone()]
    public string Phone { get; set; }
  }
}