
using lena.Services.Core.Foundation;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ProjectManagement.Exception
{
  public class ProjectERPCodeHasExistException : InternalServiceException
  {
    public string Code { get; set; }
    public ProjectERPCodeHasExistException(string code)
    {
      this.Code = code;
    }
  }
}
