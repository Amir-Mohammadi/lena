using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityControl.Exception
{
  public class QualityControlDepartmentNotDefinedException : InternalServiceException
  {
    public int StuffId { get; }

    public QualityControlDepartmentNotDefinedException()
    {
    }

    public QualityControlDepartmentNotDefinedException(int stuffId)
    {
      this.StuffId = stuffId;
    }
  }
}
