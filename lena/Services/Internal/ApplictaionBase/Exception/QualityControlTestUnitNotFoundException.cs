﻿using lena.Services.Core.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ApplictaionBase.Exception
{
  public class QualityControlTestUnitNotFoundException : InternalServiceException
  {
    public int Id { get; set; }
    public QualityControlTestUnitNotFoundException(int id)
    {
      Id = id;
    }
  }
}
