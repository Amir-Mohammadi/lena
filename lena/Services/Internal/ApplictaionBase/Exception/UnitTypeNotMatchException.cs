using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ApplictaionBase.Exception
{
  public class UnitTypeNotMatchException : InternalServiceException
  {
    public string UnitName1 { get; set; }
    public string UnitName2 { get; set; }

    public UnitTypeNotMatchException(string unitName1, string unitName2)
    {
      UnitName1 = unitName1;
      UnitName2 = unitName2;
    }
  }
}
