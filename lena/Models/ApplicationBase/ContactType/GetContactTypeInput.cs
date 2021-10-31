using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models
{
  public class GetContactTypeInput : SearchInput<ContactTypeSortType>
  {
    public TValue<int> Id { get; set; }
    public TValue<string> Name { get; set; }
    public GetContactTypeInput(PagingInput pagingInput, ContactTypeSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {

    }
  }
}