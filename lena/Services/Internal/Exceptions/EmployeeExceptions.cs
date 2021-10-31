

using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Exceptions
{
  #region Employee Exceptions

  public class EmployeeNotFoundException : InternalServiceException
  {
    public EmployeeNotFoundException(int id)
    {
      Id = id;
    }

    public int Id { get; }

    public EmployeeNotFoundException(string code)
    {
      Code = code;
    }

    public string Code { get; }
  }

  public class EmployeeCodeExsits : InternalServiceException
  {
    public EmployeeCodeExsits(string code)
    {
      this.Code = code;
    }

    public string Code { get; }
  }

  #endregion
}
