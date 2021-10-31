using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement.Exception
{
  public class NewShoppingDetailNotFoundException : InternalServiceException
  {
    public string Code { get; }

    public int Id { get; }

    public NewShoppingDetailNotFoundException(int id)
    {
      this.Id = id;
    }

    public NewShoppingDetailNotFoundException(string code)
    {
      this.Code = code;
    }
  }
}
