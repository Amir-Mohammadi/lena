﻿using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Exceptions
{
  public class StuffCodeIsNotBelongToTheProductionOrderException : InternalServiceException
  {
    public StuffCodeIsNotBelongToTheProductionOrderException()
    {
    }
  }
}


