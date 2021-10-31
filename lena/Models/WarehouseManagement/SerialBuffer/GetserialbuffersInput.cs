using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.SerialBuffer
{
  public class GetSerialBuffersInput : SearchInput<SerialBufferSortType>
  {
    public GetSerialBuffersInput(PagingInput pagingInput, SerialBufferSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
    public string Serial { get; set; }
    public int? StuffId { get; set; }
    public string StuffCode { get; set; }
    public int? WarehouseId { get; set; }
    public int? ProductionLineId { get; set; }
    public int? ProductionTerminalId { get; set; }
    public SerialBufferType? SerialBufferType { get; set; }

  }
}
