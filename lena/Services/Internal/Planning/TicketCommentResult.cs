using lena.Domains;
using lena.Domains.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using lena.Domains.Enums;
namespace lena.Models.Planning.TicketComment
{
  public class TicketCommentResult
  {
    public int Id { get; set; }
    public int UserId { get; set; }
    public int? TicketSoftwareId { get; set; }
    public DateTime CreateDateTime { get; set; }
    public DateTime ResponseDateTime { get; set; }
    public string SupporterFullName { get; set; }
    public string ContentResponse { get; set; }

    public byte[] RowVersion { get; set; }
  }
}
