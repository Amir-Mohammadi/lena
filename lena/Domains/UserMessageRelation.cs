using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class UserMessageRelation : IEntity
  {
    protected internal UserMessageRelation()
    {
    }
    public int FromUserId { get; set; }
    public int ToUserId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual User FromUser { get; set; }
    public virtual User ToUser { get; set; }
  }
}