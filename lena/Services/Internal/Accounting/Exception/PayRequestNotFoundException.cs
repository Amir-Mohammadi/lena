using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting.Exception
{
  public class PayRequestNotFoundException : InternalServiceException
  {
    public int Id { get; set; }

    public PayRequestNotFoundException(int id)
    {
      Id = id;
    }
  }
}
