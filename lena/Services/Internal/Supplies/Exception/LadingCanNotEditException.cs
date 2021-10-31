using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  public class LadingCanNotEditException : InternalServiceException
  {

    public int Id { get; }

    public LadingCanNotEditException(int id)
    {
      this.Id = id;
    }
  }
}
