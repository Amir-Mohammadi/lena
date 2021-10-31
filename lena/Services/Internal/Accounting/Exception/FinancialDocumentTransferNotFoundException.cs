﻿using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting.Exception
{
  public class FinancialDocumentTransferNotFoundException : InternalServiceException
  {
    public int Id { get; set; }

    public FinancialDocumentTransferNotFoundException(int id)
    {
      Id = id;
    }
  }
}
