
using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Exceptions
{
  #region ProjectHeader
  public class ApiInfoNotFoundException : InternalServiceException
  {
    public int Id { get; }
    public ApiInfoNotFoundException(int id)
    {
      this.Id = id;
    }
  }
  #endregion
}
