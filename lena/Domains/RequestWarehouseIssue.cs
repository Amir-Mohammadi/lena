using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class RequestWarehouseIssue : WarehouseIssue, IEntity
  {
    protected internal RequestWarehouseIssue()
    {
      this.ResponseStuffRequestItems = new HashSet<ResponseStuffRequestItem>();
    }
    public virtual ICollection<ResponseStuffRequestItem> ResponseStuffRequestItems { get; set; }
  }
}