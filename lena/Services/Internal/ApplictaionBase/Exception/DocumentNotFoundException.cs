using System;
using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ApplictaionBase.Exception
{
  public class DocumentNotFoundException : InternalServiceException
  {
    public Guid Id { get; }

    public DocumentNotFoundException(Guid id)
    {
      this.Id = id;
    }
  }
}
