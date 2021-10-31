using lena.Services.Core.Foundation;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Exceptions
{
  public class OrderItemNotFoundException : InternalServiceException
  {
    public OrderItemNotFoundException()
    {
    }

    public OrderItemNotFoundException(int id)
    {
      Id = id;
    }
    public int Id { get; }
  }
  public class StuffProjectHeaderIsNullException : InternalServiceException
  {
    public string StuffCode { get; }
    public StuffProjectHeaderIsNullException(string stuffCode)
    {
      this.StuffCode = stuffCode;
    }
  }
  public class DocumentNumberExistException : InternalServiceException
  {
    public int Id { get; }
    public DocumentNumberExistException()
    {

    }
  }
}