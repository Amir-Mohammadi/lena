using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ResponseWarehouseIssue : BaseEntity, IEntity
  {
    protected internal ResponseWarehouseIssue()
    {
    }
    public byte[] RowVersion { get; set; }
    public int WarehouseIssueId { get; internal set; }
    public virtual WarehouseIssue WarehouseIssue { get; set; }
  }
}