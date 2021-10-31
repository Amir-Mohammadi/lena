using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Common
{
  //[DataContract]
  public class SearchInput<T> : SortInput<T>
  {
    //[DataMember]
    public string SearchText { get; set; }
    //[DataMember]
    public PagingInput PagingInput { get; set; }
    //[DataMember]
    public AdvanceSearchItem[] AdvanceSearchItems { get; set; }
    public SearchInput(PagingInput pagingInput, T sortType, SortOrder sortOrder) : base(sortOrder, sortType)
    {
      this.PagingInput = pagingInput;
    }
  }
}
