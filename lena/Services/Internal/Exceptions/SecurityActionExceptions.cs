using lena.Services.Core.Foundation;


using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Exceptions
{
  #region Security Action Exceptions

  public class SecurityActionNotFoundException : InternalServiceException
  {
  }

  public class ActionParameterNotFoundException : InternalServiceException
  {
  }

  #endregion
}
