using lena.Services.Core.Foundation;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning.Exception
{
  public class BillOfMaterialNotFoundException : InternalServiceException
  {
    public int StuffId { get; }
    public int Version { get; }
    public BillOfMaterialNotFoundException(int stuffId, int version)
    {
      this.StuffId = stuffId;
      this.Version = version;
    }
  }
}
