using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Exceptions
{
  public class ReturnSerialRequestNotFoundException : InternalServiceException
  {
    public int Id { get; set; }

    public ReturnSerialRequestNotFoundException(int id)
    {
      Id = id;
    }
  }
}