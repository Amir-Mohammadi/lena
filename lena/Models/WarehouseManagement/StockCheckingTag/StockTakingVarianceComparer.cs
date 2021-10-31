using System;
using System.Collections.Generic;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StockCheckingTag
{
  public class StockTakingVarianceComparer : IEqualityComparer<StockTakingVarianceResult>
  {
    public bool Equals(StockTakingVarianceResult x, StockTakingVarianceResult y)
    {

      //Check whether the compared objects reference the same data.
      if (Object.ReferenceEquals(x, y)) return true;

      //Check whether any of the compared objects is null.
      if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
        return false;

      return x.StockCheckingId == y.StockCheckingId
             && x.StockCheckingTitle == y.StockCheckingTitle
             && x.WarehouseId == y.WarehouseId
             && x.WarehouseName == y.WarehouseName
             && x.TagTypeId == y.TagTypeId
             && x.TagTypeName == y.TagTypeName
             && x.StuffId == y.StuffId
             && x.StuffCode == y.StuffCode
             && x.StuffName == y.StuffName
             && x.StuffType == y.StuffType
             && x.StuffCategoryId == y.StuffCategoryId
             && x.StuffCategoryName == y.StuffCategoryName
             && x.UnitId == y.UnitId
             && x.UnitName == y.UnitName
             && x.StuffSerialCode == y.StuffSerialCode
             && x.Serial == y.Serial
             && x.TagAmount == y.TagAmount
             && x.TagCountingTotal == y.TagCountingTotal
             && x.StockTotal == y.StockTotal
             && x.StockSerialAmount == y.StockSerialAmount;
    }

    public int GetHashCode(StockTakingVarianceResult obj)
    {
      if (Object.ReferenceEquals(obj, null)) return 0;

      //Get hash code for the StuffId field if it is not null.
      int hashStuffId = obj.StuffId == null ? 0 : obj.StuffId.GetHashCode();

      //Get hash Code for the StuffSerialCode field.
      int hashStuffSerialCode = obj.StuffSerialCode.GetHashCode();

      //Calculate the hash code for the StockTakingVarianceResult.
      return hashStuffId ^ hashStuffSerialCode;
    }
  }
}
