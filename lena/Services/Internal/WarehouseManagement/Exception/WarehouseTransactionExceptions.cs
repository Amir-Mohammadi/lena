using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement.Exception
{
  #region WarehouseTransaction
  public class WarehouseTransactionNotFoundException : InternalServiceException
  {
    public int Id { get; }
    public WarehouseTransactionNotFoundException(int id)
    {
      this.Id = id;
    }

  }
  #endregion
}
