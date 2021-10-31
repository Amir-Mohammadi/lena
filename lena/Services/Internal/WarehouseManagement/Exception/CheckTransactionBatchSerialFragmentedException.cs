using lena.Services.Core;
using lena.Services.Core.Foundation;
using lena.Models.WarehouseManagement.UncommitedTransaction;
using System.Collections.Generic;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement.Exception
{
  public class CheckTransactionBatchSerialFragmentedException : InternalServiceException
  {
    public List<CheckTransactionBatchInfo> Info { get; set; }

    public List<UncommitedTransaction> Transactions { get; set; }
    public CheckTransactionBatchSerialFragmentedException(List<CheckTransactionBatchInfo> info)
    {
      Info = info;
      Transactions = App.Providers.UncommitedTransactionAgent.GetReport();
    }
  }
}
