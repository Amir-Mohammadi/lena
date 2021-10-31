using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ApplictaionBase.Exception
{
  public class UnitTypeNotFoundException : InternalServiceException
  {
    public int Id { get; }
    public UnitTypeNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
