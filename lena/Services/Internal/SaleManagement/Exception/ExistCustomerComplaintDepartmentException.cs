using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lena.Services.Core.Foundation;



using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.SaleManagement.Exception
{
  public class ExistCustomerComplaintDepartmentException : InternalServiceException
  {
    public int Id { get; }


    public ExistCustomerComplaintDepartmentException(int id)
    {
      this.Id = id;

    }
  }
}
