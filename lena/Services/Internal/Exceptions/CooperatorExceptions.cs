using lena.Services.Core.Foundation;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Exceptions
{
  #region Cooperator
  public class CooperatorNotFoundException : InternalServiceException
  {
    public int Id { get; }
    public CooperatorNotFoundException(int id)
    {
      Id = id;
    }
  }
  #endregion
}
