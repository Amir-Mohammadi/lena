using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.Common
{
  //[DataContract]
  public class SortInput<T>
  {
    //[DataMember]
    public SortOrder SortOrder;
    //[DataMember]
    public T SortType { get; set; }
    public SortInput(SortOrder sortOrder, T sortType)
    {
      SortOrder = sortOrder;
      SortType = sortType;
    }
  }
}
