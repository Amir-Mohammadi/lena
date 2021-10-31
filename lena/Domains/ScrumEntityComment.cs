using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ScrumEntityComment : IEntity
  {
    protected internal ScrumEntityComment()
    {
    }
    public int Id { get; set; }
    public DateTime DateTime { get; set; }
    public string Comment { get; set; }
    public int UserId { get; set; }
    public int ScrumEntityId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual User User { get; set; }
    public virtual ScrumEntity ScrumEntity { get; set; }
  }
}