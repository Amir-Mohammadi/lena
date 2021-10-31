using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning.Exception
{
  public class BillOfMaterialDetailNotFoundException : InternalServiceException
  {
    public int Id { get; }
    public int StuffId { get; }
    public int VersionId { get; }

    public BillOfMaterialDetailNotFoundException(int id, int? stuffId = null, int? versionId = null)
    {
      this.Id = id;

      if (stuffId != null)
        this.StuffId = stuffId.Value;
      if (versionId != null)
        this.VersionId = versionId.Value;
    }
  }
}
