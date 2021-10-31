using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class FinancialDocumentCorrection : IEntity
  {
    protected internal FinancialDocumentCorrection()
    {
    }
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
    public int FinancialDocumentId { get; internal set; }
    public bool IsActive { get; set; }
    public FinancialTransactionLevel FinancialTransactionLevel { get; set; }
    public virtual FinancialDocument FinancialDocument { get; set; }
  }
}
