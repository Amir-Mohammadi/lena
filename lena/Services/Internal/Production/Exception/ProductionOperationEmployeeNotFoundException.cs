using lena.Services.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Production.Exception
{
  public class ProductionOperationEmployeeNotFoundException : InternalException
  {
    public int Id { get; set; }

    public ProductionOperationEmployeeNotFoundException(int id)
    {
      Id = id;
    }
  }
}
