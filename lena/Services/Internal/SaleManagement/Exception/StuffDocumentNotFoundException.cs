using System;
using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.SaleManagement.Exception
{
  class StuffDocumentNotFoundException : InternalServiceException
  {
    public int StuffDocumentId { get; }

    public StuffDocumentNotFoundException(int id)
    {
      this.StuffDocumentId = id;
    }
  }
}