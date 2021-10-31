﻿using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.SaleManagement.Exception
{
  public class OrderItemProductionBlockNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public OrderItemProductionBlockNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}