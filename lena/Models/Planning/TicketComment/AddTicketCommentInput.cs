using lena.Domains.Enums;
namespace lena.Models.Planning.TicketComment
{
  public class AddTicketCommentInput
  {
    public string ContentResponse { get; set; }
    public int TicketSoftwareId { get; set; }
    public string IssueLink { get; set; }
  }
}