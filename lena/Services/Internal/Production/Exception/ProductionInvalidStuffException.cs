﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Production.Exception
{
  public class ProductionInvalidStuffException : InternalServiceException
  {
    public int StuffId { get; }

    public ProductionInvalidStuffException(int stuffId)
    {
      this.StuffId = stuffId;
    }
  }
}
