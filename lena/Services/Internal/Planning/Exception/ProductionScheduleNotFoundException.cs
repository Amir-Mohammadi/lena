using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning.Exception
{
  public class ProductionScheduleNotFoundException : InternalServiceException
  {
    public int Id { get; }
    public string Code { get; }
    public ProductionScheduleNotFoundException(int id)
    {
      this.Id = id;
    }
    public ProductionScheduleNotFoundException(string code)
    {
      this.Code = code;
    }
  }
}
