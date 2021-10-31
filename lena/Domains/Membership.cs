using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class Membership : IEntity
  {
    protected internal Membership()
    {
    }
    public int Id { get; set; }
    public int UserId { get; set; }
    public int UserGroupId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual User User { get; set; }
    public virtual UserGroup UserGroup { get; set; }
  }
}
