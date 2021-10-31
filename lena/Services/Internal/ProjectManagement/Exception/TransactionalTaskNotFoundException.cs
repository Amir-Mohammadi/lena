using lena.Services.Core.Foundation;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ProjectManagement.Exception
{
  public class BaseEntityNotFoundException : InternalServiceException
  {
    public int Id { get; }
    public BaseEntityNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
