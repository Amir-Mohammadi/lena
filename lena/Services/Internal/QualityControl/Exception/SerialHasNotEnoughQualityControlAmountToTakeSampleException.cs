using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityControl.Exception
{
  public class SerialHasNotEnoughQualityControlAmountToTakeSampleException : InternalServiceException
  {
    public string Serial { get; set; }

    public SerialHasNotEnoughQualityControlAmountToTakeSampleException(string serial)
    {
      this.Serial = serial;
    }
  }
}
