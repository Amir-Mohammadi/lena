﻿using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement.Exception
{

  public class CustomerStuffNotFoundException : InternalServiceException
  {
    public int Id { get; set; }
    public CustomerStuffNotFoundException(int id)
    {
      this.Id = id;

    }
  }
}
