using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Production.Exception
{
  public class CannotReplaceItemException : InternalServiceException
  {
    public int StuffId { get; set; }

    public CannotReplaceItemException(int stuffId)
    {
      StuffId = stuffId;
    }
  }
}
