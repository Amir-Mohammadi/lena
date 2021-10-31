using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  class LadingNotFoundException : InternalServiceException
  {
    public int Id { get; }
    public string Code { get; set; }
    public LadingNotFoundException(string code)
    {
      this.Code = code;
    }
    public LadingNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
