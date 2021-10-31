using lena.Services.Core.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  public class CooperatorHasFinancialAcountException : InternalServiceException
  {
    public int CooperatorId { get; set; }

    public CooperatorHasFinancialAcountException(int cooperatorId)
    {
      this.CooperatorId = cooperatorId;
    }
  }
}
