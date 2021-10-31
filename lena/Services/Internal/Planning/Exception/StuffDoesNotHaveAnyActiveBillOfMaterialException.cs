﻿using lena.Services.Core.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning.Exception
{
  public class StuffDoesNotHaveAnyActiveBillOfMaterialException : InternalServiceException
  {
    public string StuffCode { get; set; }

    public StuffDoesNotHaveAnyActiveBillOfMaterialException(string stuffCode)
    {
      StuffCode = stuffCode;
    }
  }
}
