using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  public class LadingItemDetailCanNotEditException : InternalServiceException
  {
    public int Id { get; }

    public LadingItemDetailCanNotEditException(int id)
    {
      this.Id = id;
    }
  }
}

