using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Exceptions
{
  public class CanNotDeleteBankOrderContractTypeIsUsedException : InternalServiceException
  {
    public short BankOrderContractTypeId { get; set; }

    public CanNotDeleteBankOrderContractTypeIsUsedException(short bankOrderContractTypeId)
    {
      BankOrderContractTypeId = bankOrderContractTypeId;
    }
  }
}
