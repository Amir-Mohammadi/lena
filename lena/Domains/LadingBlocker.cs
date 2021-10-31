using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class LadingBlocker : IEntity
  {
    protected internal LadingBlocker()
    {
      this.Ladings = new HashSet<Lading>();
    }
    public int Id { get; set; }
    public string Title { get; set; }
    public byte[] RowVersion { get; set; }
    public int UserGroupId { get; set; }
    public virtual ICollection<Lading> Ladings { get; set; }
    public virtual UserGroup UserGroup { get; set; }
  }
}
