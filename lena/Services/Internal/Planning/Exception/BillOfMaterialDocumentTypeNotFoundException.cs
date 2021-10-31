using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning.Exception
{
  public class BillOfMaterialDocumentTypeNotFoundException : InternalServiceException
  {
    public BillOfMaterialDocumentTypeNotFoundException(int id)
    {
      Id = id;
    }

    public int Id { get; }
  }
}
