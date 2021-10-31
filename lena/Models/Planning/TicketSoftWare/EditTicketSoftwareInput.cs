using lena.Domains.Enums;
using lena.Domains.Enums;
namespace lena.Models.Planning.TicketSoftware
{
  public class EditTicketSoftwareInput
  {
    public int Id { get; set; }
    public string Subject { get; set; }
    public string Content { get; set; }
    public TicketSoftwarePriority Priority { get; set; }
    public string IssueLink { get; set; }
    public string FileKeies { get; set; }
    public string LastedEditorUserId { get; set; }

    public byte[] RowVersion { get; set; }

  }
}
