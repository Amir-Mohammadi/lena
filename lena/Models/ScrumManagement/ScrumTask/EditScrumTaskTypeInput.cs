using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.ScrumManagement.ScrumTask
{
  public class EditScrumTaskTypeInput
  {
    public int Id { get; set; }
    public TValue<string> Name { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
