using lena.Services.Core.Foundation;


using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Exceptions
{

  public class StuffSerialNotFoundException : InternalServiceException
  {
    public long Code { get; }
    public string Serial { get; }
    public int StuffId { get; }

    public StuffSerialNotFoundException(string serial)
    {
      this.Serial = serial;
    }

    public StuffSerialNotFoundException(long code, int stuffId)
    {
      this.Code = code;
      this.StuffId = stuffId;
    }
  }

}
