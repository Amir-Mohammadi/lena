using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Production.Exception
{
  public class NotEnoughSerialBufferException : InternalServiceException
  {
    public string StuffCode { get; }
    public double NeedQty { get; set; }
    public double InSerialBufferQty { get; set; }
    public NotEnoughSerialBufferException(string stuffCode, double needQty = 0, double serialBufferQty = 0)
    {
      this.StuffCode = stuffCode;

      NeedQty = needQty;
      InSerialBufferQty = serialBufferQty;
    }
  }
}
