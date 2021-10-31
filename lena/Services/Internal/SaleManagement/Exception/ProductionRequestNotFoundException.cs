using System;
using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.SaleManagement.Exception
{
  [Serializable]
  class ProductionRequestNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public ProductionRequestNotFoundException(string message) : base(message)
    {
    }

    public ProductionRequestNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}