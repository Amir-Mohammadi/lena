using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Exceptions
{
  #region StuffCategory
  public class StuffCategoryNotFoundException : InternalServiceException
  {
    public int Id { get; }
    public StuffCategoryNotFoundException(int id)
    {
      Id = id;
    }

  }
  #endregion
}
