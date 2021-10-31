﻿using lena.Services.Core.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement.Exception
{
  public class YouMustAddAllItemsMatchingTheSelectedItemsToTheListException : InternalServiceException
  {
    public string Code { get; set; }

    public YouMustAddAllItemsMatchingTheSelectedItemsToTheListException(string code)
    {
      this.Code = code;
    }
  }
}
