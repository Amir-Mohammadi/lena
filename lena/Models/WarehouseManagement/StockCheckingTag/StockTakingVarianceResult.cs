using System;
using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StockCheckingTag
{
  public class StockTakingVarianceResult
  {
    public int? Id { get; set; }
    public int StockCheckingId { get; set; }
    public string StockCheckingTitle { get; set; }
    public short WarehouseId { get; set; }
    public string WarehouseName { get; set; }
    public int TagTypeId { get; set; }
    public string TagTypeName { get; set; }
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public StuffType StuffType { get; set; }
    public int StuffCategoryId { get; set; }
    public string StuffCategoryName { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public long? StuffSerialCode { get; set; }
    public string Serial { get; set; }
    public double TagAmount { get; set; }
    public double TagCountingTotal { get; set; }
    public double StockTotal { get; set; }
    public double StockSerialAmount { get; set; }
    public double ContradictionAmountRialPrice => UnitRialPrice * ContradictionAmount * UnitConversionRatio; // معادل ریالی مغایرت سریال
    public double ContradictionTotal => Math.Abs(StockTotal - TagCountingTotal); // مغایرت کالا
    public double ContradictionAmount => Math.Abs(StockSerialAmount - TagAmount); // مغایرت سریال
    public StockCheckingTagStatus StockCheckingTagStatus { get; set; }
    public bool IsReCount { get; set; }
    public QtyCorrectionRequestStatus? QtyCorrectionRequestStatus { get; set; }

    public double UnitConversionRatio { get; set; }
    public double UnitRialPrice { get; set; } // قیمت ریالی واحد
  }
}
