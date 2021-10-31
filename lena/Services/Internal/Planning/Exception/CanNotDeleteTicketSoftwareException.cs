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
  public class CanNotDeleteTicketSoftwareException : InternalServiceException
  {
    public int Id { get; set; }
    public CanNotDeleteTicketSoftwareException(int id)
    {
      Id = id;
    }
  }
}


