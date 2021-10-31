using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ApplictaionBase.Exception
{
  public class DownloadBillOfMaterialDocumentTypeAccessDeniedExecption : InternalServiceException
  {
    public int DocumentTypeId { get; }

    public DownloadBillOfMaterialDocumentTypeAccessDeniedExecption(int documentTypeId)
    {
      this.DocumentTypeId = documentTypeId;
    }
  }
}
