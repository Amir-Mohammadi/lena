﻿using lena.Services.Core.Foundation;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning.Exception
{
  public class BillOfMaterialPublishRequestNotFoundException : InternalServiceException
  {
    public int Id { get; }
    public BillOfMaterialPublishRequestNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
