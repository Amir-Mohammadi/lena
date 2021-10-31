using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Reports.Exception
{
  public class ReportIsExistException : InternalServiceException
  {
    public string Name { get; }

    public ReportIsExistException(string name)
    {
      this.Name = name;
    }
  }
}
