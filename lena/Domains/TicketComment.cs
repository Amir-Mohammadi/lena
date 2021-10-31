using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class TicketComment : IEntity
  {
    protected internal TicketComment()
    {
    }
    public int Id { get; set; }
    public int UserId { get; set; }
    public int? TicketSoftwareId { get; set; }
    public DateTime CreateDateTime { get; set; }
    public string Content { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual User User { get; set; }
    public virtual TicketSoftware TicketSoftware { get; set; }
  }
}