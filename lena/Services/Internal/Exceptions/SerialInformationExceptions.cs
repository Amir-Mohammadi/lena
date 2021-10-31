using lena.Services.Core.Foundation;


using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Exceptions
{

  public class SerialInformationNotFoundException : InternalServiceException
  {
    public string Serial { get; }

    public SerialInformationNotFoundException(string serial)
    {
      this.Serial = serial;
    }


  }

}
