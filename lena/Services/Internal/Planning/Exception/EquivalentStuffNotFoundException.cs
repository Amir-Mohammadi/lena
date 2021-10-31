﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning.Exception
{
  public class EquivalentStuffNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public EquivalentStuffNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
