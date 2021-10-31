using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityAssurance.Exception
{
  public class WeightDayNotFoundException : InternalServiceException
  {
    public int Id { get; set; }
    public string Code { get; set; }

    public WeightDayNotFoundException(int id)
    {
      this.Id = id;
    }
    public WeightDayNotFoundException(string code)
    {
      this.Code = code;
    }
  }
}
