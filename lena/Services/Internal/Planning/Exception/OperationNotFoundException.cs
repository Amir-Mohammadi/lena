using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning.Exception
{
  public class OperationNotFoundException : InternalServiceException
  {
    public OperationNotFoundException(int id)
    {
      Id = id;
    }
    public OperationNotFoundException(string code)
    {
      Code = code;
    }

    public int Id { get; }

    public string Code { get; }
  }
}
