using lena.Services.Core.Foundation;
using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement.Exception
{
  public class RollBackTransactionTypeNotFound : InternalServiceException
  {
    public int TransactionTypeId { get; }

    public RollBackTransactionTypeNotFound(int transactionTypeId)
    {
      this.TransactionTypeId = transactionTypeId;
    }
  }
}
