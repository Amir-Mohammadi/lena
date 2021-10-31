using System;
using System.Linq;
using System.Linq.Expressions;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Domains;
using lena.Models.Guard.InboundCargoCooperator;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Guard
{
  public partial class Guard
  {
    public InboundCargoCooperator GetInboundCargoCooperator(
        int cooperatorId,
        int inboundCargoId)
    {

      return repository.GetQuery<InboundCargoCooperator>()
                .FirstOrDefault(x => x.CooperatorId == cooperatorId && x.InboundCargoId == inboundCargoId);
    }

    public IQueryable<TResult> GetInboundCargoCooperators<TResult>(
        Expression<Func<InboundCargoCooperator, TResult>> selector,
        int? inboundCargoId = null)
    {

      var query = repository.GetQuery<InboundCargoCooperator>();
      if (inboundCargoId.HasValue)
      {
        query = query.Where(x => x.InboundCargoId == inboundCargoId);
      }

      return query.Select(selector);
    }
    public InboundCargoCooperator AddInboundCargoCooperator(
        int cooperatorId,
        int inboundCargoId
        )
    {

      var cp = repository.Create<InboundCargoCooperator>();
      cp.InboundCargoId = inboundCargoId;
      cp.CooperatorId = cooperatorId;
      repository.Add(cp);
      return cp;
    }

    public InboundCargoCooperator EditInboundCargoCooperator(int cooperatorId, int inboundCargoId, byte[] rowVersion)
    {

      var entity = GetInboundCargoCooperator(cooperatorId, inboundCargoId);
      entity.CooperatorId = cooperatorId;
      entity.InboundCargoId = inboundCargoId;
      repository.Update<InboundCargoCooperator>(entity, rowVersion);
      return entity;
    }

    public void DeleteInboundCargoCooperator(int cooperatorId, int inboundCargoId)
    {

      var entity = GetInboundCargoCooperator(cooperatorId, inboundCargoId);
      repository.Delete(entity);
    }



    #region ToResult
    public Expression<Func<InboundCargoCooperator, InboundCargoCooperatorResult>> ToInboundCargoCooperatorResult =
        inboundCargoCooperator => new InboundCargoCooperatorResult
        {
          CooperatorId = inboundCargoCooperator.CooperatorId,
          InboundCargoId = inboundCargoCooperator.InboundCargoId,
          CooperatorName = inboundCargoCooperator.Cooperator.Name,
          CooperatorCode = inboundCargoCooperator.Cooperator.Code,
          RowVersion = inboundCargoCooperator.RowVersion
        };
    #endregion
  }
}
