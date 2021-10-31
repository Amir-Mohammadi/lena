using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Exceptions
{
  #region Permission Exception

  public abstract class PermissonException : InternalServiceException
  {
    protected PermissonException(int id)
    {
      Id = id;
    }

    public int Id { get; }
  }

  public class PermissoinNotFoundException : PermissonException
  {
    public PermissoinNotFoundException(int id) : base(id)
    {
    }

  }

  public class UserNotLoginException : InternalServiceException
  {

  }
  #endregion
}
