using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class UserMessageSend : MessageSend, IEntity
  {
    protected internal UserMessageSend()
    {
    }
  }
}