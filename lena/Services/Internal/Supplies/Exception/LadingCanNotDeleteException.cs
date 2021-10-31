using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  public class LadingCanNotDeleteException : InternalServiceException
  {

    public int Id { get; }

    public LadingCanNotDeleteException(int id)
    {
      this.Id = id;
    }
  }
}
