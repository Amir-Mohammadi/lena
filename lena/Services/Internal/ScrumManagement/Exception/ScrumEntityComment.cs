using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ScrumManagement.Exception
{
  public class ScrumEntityCommentNotFoundException : InternalServiceException
  {
    public int Id { get; }
    public ScrumEntityCommentNotFoundException(int id)
    {
      this.Id = id;
    }

  }

}
