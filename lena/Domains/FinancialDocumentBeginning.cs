using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class FinancialDocumentBeginning : IEntity
  {
    protected internal FinancialDocumentBeginning()
    {
    }
    public int Id { get; set; }
    public int FinancialDocumentId { get; set; }
    public byte[] RowVersion { get; set; }
    public FinancialTransactionLevel FinancialTransactionLevel { get; set; }
    public virtual FinancialDocument FinancialDocument { get; set; }
  }
}
