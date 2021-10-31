using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ProjectWorkItem : ScrumTask, IEntity
  {
    protected internal ProjectWorkItem()
    {
    }
  }
}
