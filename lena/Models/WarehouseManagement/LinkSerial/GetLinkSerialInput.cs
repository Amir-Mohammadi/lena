using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.LinkSerial
{
  public class GetLinkSerialsInput : SearchInput<LinkSerialSortType>
  {
    public GetLinkSerialsInput(PagingInput pagingInput, LinkSerialSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {

    }
    public int? StuffId { get; set; }
    public int? CustomerId { get; set; }
    public string ToSerial { get; set; }
    public string FromSerial { get; set; }
    public LinkSerialType? LinkSerialType { get; set; }
    public string[] LinkedSerials { get; set; }
    public int? BillOfMaterialVersion { get; set; }
    public string LinkedSerial { get; set; }
    public bool? Printed { get; set; }

  }
}

