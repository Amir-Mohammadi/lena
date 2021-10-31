using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement.Exception
{
  public class NotDefinedCooperatorFinancialAccountException : InternalServiceException
  {
    public string CooperatorName { get; }

    public NotDefinedCooperatorFinancialAccountException(string cooperatorName)
    {
      this.CooperatorName = cooperatorName;
    }
  }
}
