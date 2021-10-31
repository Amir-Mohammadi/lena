using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  public class BankOrderIssuNumberExistException : InternalServiceException
  {

    public string Number { get; }

    public BankOrderIssuNumberExistException(string number)
    {
      this.Number = number;
    }


  }
}
