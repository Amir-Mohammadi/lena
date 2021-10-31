using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class CooperatorFinancialAccount : FinancialAccount, IEntity
  {
    protected internal CooperatorFinancialAccount()
    {
    }
    public int CooperatorId { get; set; }
    public virtual Cooperator Cooperator { get; set; }
  }
}
