using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ProductRequest : Project, IEntity
  {
    protected internal ProductRequest()
    {
    }
    public bool Response { get; set; }
  }
}
