﻿using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning.Exception
{
  public class ProductionRequestIdIsNullException : InternalServiceException
  {
    public ProductionRequestIdIsNullException()
    {
    }
  }
}
