using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class TicketSoftware : IEntity
  {
    protected internal TicketSoftware()
    {
      this.TicketFiles = new HashSet<TicketFile>();
    }
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Subject { get; set; }
    public string Content { get; set; }
    public DateTime CreateDateTime { get; set; }
    public DateTime? UpdateDateTime { get; set; }
    public string IssueLink { get; set; }
    public TicketSoftwarePriority Priority { get; set; }
    public TicketSoftwareStatus Status { get; set; }
    public int? LastedEditorUserId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual User LastedEditorUser { get; set; }
    public virtual User User { get; set; }
    public virtual ICollection<TicketFile> TicketFiles { get; set; }
    public virtual ICollection<TicketComment> TicketComments { get; set; }
  }
}