using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Exceptions
{
  #region StuffCategory
  public class StuffHSGroupNotFoundException : InternalServiceException
  {
    public int Id { get; }
    public StuffHSGroupNotFoundException(int id)
    {
      Id = id;
    }

  }
  #endregion
}
