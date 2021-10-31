﻿using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  public class PurchaseStepNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public PurchaseStepNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
