
using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  public class InLadingCustomhouseStatusLadingTypeShouldBeChange : InternalServiceException
  {
    public int LadingId { get; set; }

    public InLadingCustomhouseStatusLadingTypeShouldBeChange(int ladingId)
    {
      this.LadingId = ladingId;
    }
  }
}
