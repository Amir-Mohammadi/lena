using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ScrumEntityLog : IEntity
  {
    protected internal ScrumEntityLog()
    {
    }
    public int Id { get; set; }
    public int ScrumEntityId { get; set; }
    public string FieldName { get; set; }
    public string OldValue { get; set; }
    public string NewValue { get; set; }
    public DateTime DateTime { get; set; }
    public int UserId { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ScrumEntity ScrumEntity { get; set; }
    public virtual User User { get; set; }
  }
}