using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class BankOrderIssueType : IEntity, IHasRowVersion
  {
    protected internal BankOrderIssueType()
    {
      this.BankOrderIssues = new HashSet<BankOrderIssue>();
    }
    public int Id { get; set; }
    public string Name { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<BankOrderIssue> BankOrderIssues { get; set; }
  }
}