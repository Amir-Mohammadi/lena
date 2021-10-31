using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting.Exception
{
  public class CargoCostNotFoundException : InternalServiceException
  {
    public int Id { get; set; }

    public CargoCostNotFoundException(int id)
    {
      Id = id;
    }
  }
}
