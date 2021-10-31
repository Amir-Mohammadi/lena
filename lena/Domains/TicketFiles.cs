using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class TicketFile : IEntity
  {
    protected internal TicketFile()
    {
    }
    public int Id { get; set; }
    public int TicketSoftWareId { get; set; }
    public Guid DocumentId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual TicketSoftware TicketSoftware { get; set; }
  }
}