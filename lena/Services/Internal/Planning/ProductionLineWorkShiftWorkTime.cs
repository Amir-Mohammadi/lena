using System.Linq;
using lena.Models.Planning.ProductionLine;
using lena.Models.Planning.ProductionLineWorkShift;
using lena.Models.Planning.ProductionLineWorkShiftWorkTime;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning
{
  public partial class Planning
  {
    #region ToProductionLineWorkShiftWorkTimesResult
    public IQueryable<ProductionLineWorkShiftWorkTimesResult> ToProductionLineWorkShiftWorkTimesResultQuery(
      IQueryable<ProductionLineWorkTimeResult> productionLineWorkTimeResultQuery,
        IQueryable<ProductionLineResult> productionLineResultQuery)
    {
      #region Group
      var gwswt = from item in productionLineWorkTimeResultQuery
                  group item by item.ProductionLineId
                  into gItems
                  select new
                  {
                    ProductionLineId = gItems.Key,
                    ProductionLineWorkTimes = gItems
                  };
      #endregion
      #region CreateResult
      var result = from productionline in productionLineResultQuery
                   join twswt in gwswt on productionline.Id equals twswt.ProductionLineId into wswts
                   from wswt in wswts.DefaultIfEmpty()
                   select new ProductionLineWorkShiftWorkTimesResult
                   {
                     ProductionLineId = productionline.Id,
                     ProductionLineName = productionline.Name,
                     ProductionLineWorkTimes = wswt.ProductionLineWorkTimes.OrderBy(i => i.DateTime).AsQueryable()
                   };
      #endregion
      return result;
    }
    #endregion
  }
}
