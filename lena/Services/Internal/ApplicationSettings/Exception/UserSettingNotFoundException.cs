

using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ApplicationSettings.Exception
{
  #region Application Settings Exceptions
  public class UserSettingNotFoundException : InternalServiceException
  {
    public int Id { get; }
    public UserSettingNotFoundException(int id)
    {
      this.Id = id;
    }
  }


  #endregion
}
