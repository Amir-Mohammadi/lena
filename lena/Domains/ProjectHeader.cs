using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ProjectHeader : ScrumProjectGroup, IEntity
  {
    protected internal ProjectHeader()
    {
    }
    public int OwnerCustomerId { get; set; }
    public virtual Cooperator OwnerCustomer { get; set; }
    public virtual Stuff Stuff { get; set; }
  }
}
