using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Exceptions
{
  #region Stuff
  public class CanNotEditUnitTypeStuffException : InternalServiceException
  {
    public string Code { get; }
    public CanNotEditUnitTypeStuffException(string code)
    {
      this.Code = code;
    }

  }
  #endregion
}
