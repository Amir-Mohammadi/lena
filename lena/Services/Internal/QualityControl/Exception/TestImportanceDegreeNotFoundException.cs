using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityControl.Exception
{
  public class TestImportanceDegreeNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public TestImportanceDegreeNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}