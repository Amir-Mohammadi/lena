using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityControl.Exception
{
  public class TestOperationNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public TestOperationNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}