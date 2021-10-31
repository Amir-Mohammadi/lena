using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models
{
  public class GetContactsInput : SearchInput<ContactSortType>
  {
    public bool IsEmployeeContact { get; set; }
    public bool IsProviderContact { get; set; }
    public bool IsCustomerContact { get; set; }
    public int? ContactTypeId { get; set; }
    public GetContactsInput(PagingInput pagingInput, ContactSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}