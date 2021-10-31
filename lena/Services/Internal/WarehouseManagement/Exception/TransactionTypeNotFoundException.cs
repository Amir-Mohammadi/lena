using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement.Exception
{

  #region TransactionType
  public class TransactionTypeNotFoundException : InternalServiceException
  {
    public int Id { get; }
    public TransactionTypeNotFoundException(int id)
    {
      this.Id = id;
    }

  }
  #endregion
}
