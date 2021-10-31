using lena.Services.Core;
using lena.Services.Core.Foundation;
using lena.Models.WarehouseManagement.UncommitedTransaction;
using System.Collections.Generic;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement.Exception
{
  public class CheckTransactionBatchException : InternalServiceException
  {
    private CheckTransactionBatchInfo[] checkTransactionBatchInfo;

    public List<CheckTransactionBatchInfo> Info { get; set; }
    public List<UncommitedTransaction> Transactions { get; set; }


    public CheckTransactionBatchException(List<CheckTransactionBatchInfo> info)
    {
      Info = info;
      Transactions = App.Providers.UncommitedTransactionAgent.GetReport();
    }

    public CheckTransactionBatchException(CheckTransactionBatchInfo[] checkTransactionBatchInfo)
    {
      this.checkTransactionBatchInfo = checkTransactionBatchInfo;
    }
  }
}
