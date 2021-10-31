using lena.Services.Core.Foundation;


using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Exceptions
{
  #region SerialProfile
  public class SerialProfileNotFoundException : InternalServiceException
  {
    public int Code { get; }
    public int StuffId { get; }
    public SerialProfileNotFoundException(int code, int stuffId)
    {
      this.Code = code;
      this.StuffId = stuffId;
    }
  }
  #endregion
}
