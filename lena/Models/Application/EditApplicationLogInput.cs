using lena.Domains.Enums;
namespace lena.Models.Application
{
  public class EditApplicationLogInput : AddApplicationLogInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
