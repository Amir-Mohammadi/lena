using lena.Domains;
using lena.Domains.Enums;
using lena.Models.Planning.TicketComment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using lena.Domains.Enums;
namespace lena.Models.Planning.TicketSoftware
{
  public class TicketSoftwareResult
  {
    public int Id { get; set; }
    public int UserId { get; set; }
    public string UserFullName { get; set; }
    public string Subject { get; set; }
    public string Content { get; set; }
    public DateTime CreateDateTime { get; set; }
    public DateTime? UpdateDateTime { get; set; }
    public TicketSoftwareStatus Status { get; set; }
    public string IssueLink { get; set; }
    public int FileId { get; set; }
    public TicketSoftwarePriority Priority { get; set; }
    public int? LastedEditorUserId { get; set; }
    public string UserFullNameEditor { get; set; }

    public IQueryable<TicketCommentResult> TicketComments { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
