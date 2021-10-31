using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class LadingCustomhouseLog : IEntity
  {
    protected internal LadingCustomhouseLog()
    {
    }
    public int Id { get; set; }
    public int LadingId { get; set; }
    public int LadingCustomhouseStatusId { get; set; }
    public string Description { get; set; }
    public DateTime DateTime { get; set; }
    public byte[] RowVersion { get; set; }
    public int UserId { get; set; }
    public virtual LadingCustomhouseStatus LadingCustomhouseStatus { get; set; }
    public virtual Lading Lading { get; set; }
    public virtual User User { get; set; }
  }
}
