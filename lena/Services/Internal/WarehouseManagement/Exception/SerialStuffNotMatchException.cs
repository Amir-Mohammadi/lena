using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement.Exception
{
  public class SerialStuffNotMatchException : InternalServiceException
  {
    public string Serial { get; }
    public string SerialStuffCode { get; }
    public string StuffCode { get; }
    public SerialStuffNotMatchException(string serial, string serialStuffCode, string stuffCode)
    {
      this.Serial = serial;
      this.SerialStuffCode = serialStuffCode;
      this.StuffCode = stuffCode;
    }
  }
}
