using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting.Exception
{
  public class CostCenterCanNotEdit : InternalServiceException
  {
    public int Id { get; set; }

    public CostCenterCanNotEdit(int id)
    {
      Id = id;
    }
  }
}
