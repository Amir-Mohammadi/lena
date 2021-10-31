using lena.Services.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.WarehouseManagement.Exception;
using lena.Models.Common;
using lena.Domains;
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

    #region Get
    public StuffRequestMilestoneDetailSummary GetStuffRequestMilestoneDetailSummary(int id) => GetStuffRequestMilestoneDetailSummary(selector: e => e, id: id);
    public TResult GetStuffRequestMilestoneDetailSummary<TResult>(
        Expression<Func<StuffRequestMilestoneDetailSummary, TResult>> selector,
        int id)
    {

      var stuffRequestMilestoneDetailSummary = GetStuffRequestMilestoneDetailSummarys(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (stuffRequestMilestoneDetailSummary == null)
        throw new StuffRequestMilestoneDetailSummaryNotFoundException(id);
      return stuffRequestMilestoneDetailSummary;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetStuffRequestMilestoneDetailSummarys<TResult>(
            Expression<Func<StuffRequestMilestoneDetailSummary, TResult>> selector,
            TValue<int> id = null,
            TValue<double> orderdQty = null,
            TValue<double> cargoedQty = null,
            TValue<double> reciptedQty = null,
            TValue<double> qualityControlPassedQty = null

            )
    {

      var query = repository.GetQuery<StuffRequestMilestoneDetailSummary>();
      if (id != null) query = query.Where(i => i.Id == id);
      if (orderdQty != null) query = query.Where(x => x.OrderedQty == orderdQty);
      if (reciptedQty != null) query = query.Where(x => x.ReciptedQty == reciptedQty);
      if (qualityControlPassedQty != null) query = query.Where(x => x.QualityControlPassedQty == qualityControlPassedQty);

      return query.Select(selector);
    }
    #endregion
    #region Add
    public StuffRequestMilestoneDetailSummary AddStuffRequestMilestoneDetailSummary(

    int stuffRequestMilestoneDetailId,
                double orderdQty,
                double cargoedQty,
                double reciptedQty,
                double qualityControlPassedQty
                )
    {

      var stuffRequestMilestoneDetail =
    GetStuffRequestMilestoneDetail(stuffRequestMilestoneDetailId);
      var stuffRequestMilestoneDetailSummary = repository.Create<StuffRequestMilestoneDetailSummary>();
      stuffRequestMilestoneDetailSummary.OrderedQty = orderdQty;
      stuffRequestMilestoneDetailSummary.StuffRequestMilestoneDetail = stuffRequestMilestoneDetail;
      stuffRequestMilestoneDetailSummary.CargoedQty = cargoedQty;
      stuffRequestMilestoneDetailSummary.ReciptedQty = reciptedQty;
      stuffRequestMilestoneDetailSummary.QualityControlPassedQty = qualityControlPassedQty;
      repository.Add(stuffRequestMilestoneDetailSummary);
      return stuffRequestMilestoneDetailSummary;
    }
    #endregion
    #region Edit
    public StuffRequestMilestoneDetailSummary EditStuffRequestMilestoneDetailSummary(
        int id,
        byte[] rowVersion,
        TValue<double> orderdQty = null,
        TValue<double> cargoedQty = null,
        TValue<double> reciptedQty = null,
        TValue<double> qualityControlPassedQty = null
        )
    {

      var stuffRequestMilestoneDetailSummary = GetStuffRequestMilestoneDetailSummary(id: id);
      return EditStuffRequestMilestoneDetailSummary(
                 stuffRequestMilestoneDetailSummary: stuffRequestMilestoneDetailSummary,
                 rowVersion: rowVersion,
                 orderdQty: orderdQty,
                 cargoedQty: cargoedQty,
                 qualityControlPassedQty: qualityControlPassedQty,
                 reciptedQty: reciptedQty
                );

    }

    public StuffRequestMilestoneDetailSummary EditStuffRequestMilestoneDetailSummary(
                StuffRequestMilestoneDetailSummary stuffRequestMilestoneDetailSummary,
                byte[] rowVersion,
                TValue<double> orderdQty = null,
                TValue<double> cargoedQty = null,
                TValue<double> reciptedQty = null,
                TValue<double> qualityControlPassedQty = null


                )
    {


      if (orderdQty != null) stuffRequestMilestoneDetailSummary.OrderedQty = orderdQty;
      if (cargoedQty != null) stuffRequestMilestoneDetailSummary.CargoedQty = cargoedQty;
      if (reciptedQty != null) stuffRequestMilestoneDetailSummary.ReciptedQty = reciptedQty;
      if (qualityControlPassedQty != null) stuffRequestMilestoneDetailSummary.QualityControlPassedQty = qualityControlPassedQty;

      repository.Update(stuffRequestMilestoneDetailSummary, rowVersion);
      return stuffRequestMilestoneDetailSummary;
    }

    #endregion
    #region Delete
    public void DeleteStuffRequestMilestoneDetailSummary(int id)
    {

      var stuffRequestMilestoneDetailSummary = GetStuffRequestMilestoneDetailSummary(id: id);
      repository.Delete(stuffRequestMilestoneDetailSummary);
    }
    #endregion

    #region Reset

    public StuffRequestMilestoneDetailSummary ResetStuffRequestMilestoneDetailSummary(int id)
    {

      var purchaseRequestSummary = GetStuffRequestMilestoneDetailSummary(id: id); ; return ResetStuffRequestMilestoneDetailSummary(summary: null);

    }
    public StuffRequestMilestoneDetailSummary ResetStuffRequestMilestoneDetailSummary(StuffRequestMilestoneDetailSummary summary)
    {



      StuffRequestMilestoneDetailSummary stuffRequestMilestoneDetailSummary = null;
      return stuffRequestMilestoneDetailSummary;
    }

    #endregion


  }
}

