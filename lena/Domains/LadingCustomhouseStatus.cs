using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class LadingCustomhouseStatus : IEntity
  {
    protected internal LadingCustomhouseStatus()
    {
      this.LadingCustomhouseLogs = new HashSet<LadingCustomhouseLog>();
    }
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<LadingCustomhouseLog> LadingCustomhouseLogs { get; set; }
  }
}
