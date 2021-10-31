﻿using lena.Services.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting.Exception
{
  public class CannotModifiedFinanceAllocationInThisStatusException : InternalException
  {
    public string code { get; set; }

    public CannotModifiedFinanceAllocationInThisStatusException(string code)
    {
      this.code = code;
    }
  }
}
