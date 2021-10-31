using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;
using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.IranKhodroSerial
{
  public class GetIranKhodroSerialsInput : SearchInput<IranKhodroSerialSortType>
  {
    public GetIranKhodroSerialsInput(PagingInput pagingInput, IranKhodroSerialSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {

    }
    public int? StuffId { get; set; }
    public string Serial { get; set; }
    public int? CustomerStuffId { get; set; }
    public int? ProductionYearId { get; set; }
    public int? CustomerStuffVersionId { get; set; }
    public string ManufacturerCode { get; set; }
    public DateTime? ProductionDateTime { get; set; }
    public string[] Serials { get; set; }
    public string ToSerial { get; set; }
    public string FromSerial { get; set; }

  }
}

