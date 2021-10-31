using lena.Domains.Enums;
namespace lena.Models
{
  public class AddReturnSerialToPreviousStateRequestInput
  {
    public string Serial { get; set; }
    public int WrongDoerUserId { get; set; }
    public string Description { get; set; }
  }
}