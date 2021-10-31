using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ScrumManagement.Exception
{
  public class ScrumEntityNotFoundException : InternalServiceException
  {
    public string Code { get; }

    public int Id { get; }
    public ScrumEntityNotFoundException(int id)
    {
      this.Id = id;
    }

    public ScrumEntityNotFoundException(string code)
    {
      this.Code = code;
    }
  }
}
