using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class BaseEntityLog : IEntity
  {
    protected internal BaseEntityLog()
    {
    }
    public int Id { get; set; }
    public int BaseEntityId { get; set; }
    public int UserId { get; set; }
    public DateTime DateTime { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual BaseEntity BaseEntity { get; set; }
    public virtual User User { get; set; }
  }
}