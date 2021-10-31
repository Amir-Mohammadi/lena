using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Exceptions
{
  #region Stuff
  public class StuffNotFoundException : InternalServiceException
  {
    public int Id { get; }
    public StuffNotFoundException(int id)
    {
      this.Id = id;
    }

  }
  #endregion
}
