using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ApplictaionBase.Exception
{
  public class UnitNotFoundException : InternalServiceException
  {
    public int Id { get; }
    public UnitNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
