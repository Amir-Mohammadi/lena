using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning.Exception
{
  public class EmployeeWorkReportNotFoundException : InternalServiceException
  {
    public int UserId { get; set; }

    public EmployeeWorkReportNotFoundException(int userId)
    {
      this.UserId = userId;
    }
  }
}
