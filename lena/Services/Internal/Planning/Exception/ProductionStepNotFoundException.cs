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
  public class ProductionStepNotFoundException : InternalServiceException
  {
    public ProductionStepNotFoundException(int id)
    {
      Id = id;
    }
    public int Id { get; }
  }
}
