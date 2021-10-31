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
  public class RepairProductionStuffDetailNotFoundException : InternalException
  {
    public int RepairProductionStuffDetailId { get; set; }

    public RepairProductionStuffDetailNotFoundException(int id)
    {
      this.RepairProductionStuffDetailId = id;
    }
  }
}
