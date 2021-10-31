using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class TerminalTicketRegistery : IEntity
  {
    protected internal TerminalTicketRegistery()
    {
    }
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string SessionId { get; set; }
    public string Value { get; set; }
    public byte[] RowVersion { get; set; }
  }
}