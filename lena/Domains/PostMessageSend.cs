using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class PostMessageSend : MessageSend, IEntity
  {
    protected internal PostMessageSend()
    {
    }
    public int PostId { get; set; }
    public virtual Post Post { get; set; }
  }
}
