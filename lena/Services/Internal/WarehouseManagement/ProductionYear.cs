using lena.Services.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.SaleManagement.Exception;
using lena.Services.Internals.WarehouseManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models;
using lena.Models.Common;
using lena.Models.Supplies.ProductionYear;
using System;
using System.Linq;
using System.Linq.Expressions;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {
    #region Add
    public ProductionYear AddProductionYear(
        string code,
        DateTime year)
    {

      var productionYear = repository.Create<ProductionYear>();
      productionYear.Code = code.ToUpper();
      productionYear.Year = year;
      repository.Add(productionYear);
      return productionYear;
    }
    #endregion

    #region Edit

    public ProductionYear EditProductionYearProcess(
           byte[] rowVersion,
           int id,
           TValue<string> code = null,
           TValue<DateTime> year = null
           )
    {

      #region Check ProductionYearUsedInSerial
      var productionYearRes = GetProductionYear(id: id);

      var iranKhodroResult = GetIranKhodroSerials(e => e,
                productionYearId: productionYearRes.Id);
      if (iranKhodroResult.Any())
      {
        throw new ProducstionYearHasBeenUsedInMakingIranKhodroSerial(productiojYearId: id, code: productionYearRes.Code);
      }
      #endregion
      var productionYear = EditProductionYear(
                    id: id,
                    code: code,
                    year: year,
                    rowVersion: rowVersion);

      return productionYear;
    }

    public ProductionYear EditProductionYear(
        byte[] rowVersion,
        int id,
        TValue<string> code = null,
        TValue<DateTime> year = null
        )
    {

      var productionYear = GetProductionYear(id: id);
      if (code != null)
        productionYear.Code = code.Value.ToUpper();
      if (year != null)
        productionYear.Year = year;
      repository.Update(productionYear, rowVersion);
      return productionYear;
    }
    #endregion

    #region Delete
    public void DeleteProductionYear(int id)
    {

      var productionYear = GetProductionYear(id: id);
      repository.Delete(productionYear);
    }
    #endregion


    #region Get
    public ProductionYear GetProductionYear(int id) => GetProductionYear(selector: e => e, id: id);
    internal TResult GetProductionYear<TResult>(
    Expression<Func<ProductionYear, TResult>> selector,
    int id)
    {


      var productionYear = GetProductionYears(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (productionYear == null)
        throw new ProductionYearNotFoundException(id: id);
      return productionYear;

    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetProductionYears<TResult>(
        Expression<Func<ProductionYear, TResult>> selector,
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<DateTime> year = null
      )
    {


      var productionYear = repository.GetQuery<ProductionYear>();

      if (id != null)
        productionYear = productionYear.Where(i => i.Id == id);
      if (code != null)
        productionYear = productionYear.Where(i => i.Code == code);

      if (year != null)
        productionYear = productionYear.Where(i => i.Year == year);

      return productionYear.Select(selector);
    }
    #endregion

    #region ToResult
    public Expression<Func<ProductionYear, ProductionYearResult>> ToProductionYearResult =
     productionYear => new ProductionYearResult
     {
       Id = productionYear.Id,
       Code = productionYear.Code,
       Year = productionYear.Year,
       RowVersion = productionYear.RowVersion
     };

    #endregion

    #region Search
    public IQueryable<ProductionYearResult> SearchProductionYear(IQueryable<ProductionYearResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {

      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = query.Where(item =>
            item.Code.Contains(searchText) ||
            item.Year.ToString().Contains(searchText));
      }

      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      return query;
    }
    #endregion

    #region Sort
    public IOrderedQueryable<ProductionYearResult> SortProductionYearResult(IQueryable<ProductionYearResult> query,
        SortInput<ProductionYearSortType> sort)
    {
      switch (sort.SortType)
      {
        case ProductionYearSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case ProductionYearSortType.Code:
          return query.OrderBy(a => a.Code, sort.SortOrder);
        case ProductionYearSortType.Year:
          return query.OrderBy(a => a.Year, sort.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region ToProductionYearComboList
    public IQueryable<ProductionYearComboResult> ToProductionYearComboList(IQueryable<ProductionYear> input)
    {
      return from productionYear in input
             select new ProductionYearComboResult()
             {
               Id = productionYear.Id,
               Code = productionYear.Code,
               Year = productionYear.Year
             };
    }
    #endregion

  }
}
