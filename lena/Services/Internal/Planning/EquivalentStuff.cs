using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using System.Linq;
using lena.Services.Common;
////using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Planning.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.Planning.BillOfMaterialDetail;
using lena.Models.Planning.EquivalentStuff;
using lena.Models.Planning.EquivalentStuffDetail;
using lena.Services.Core.Common;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning
{
  public partial class Planning
  {
    public IQueryable<EquivalentStuff> GetEquivalentStuffs(
        TValue<int> id = null,
        TValue<int> billOfMaterialDetailId = null,
        TValue<string> title = null,
        TValue<string> description = null,
        TValue<bool> isActive = null,
        TValue<EquivalentStuffType> equivalentStuffType = null,
        TValue<int> billOfMaterialStuffId = null,
        TValue<int> billOfMaterialVersion = null,
        TValue<int> billOfMaterialDetailStuffId = null
        )
    {
      var equivalentStuff = repository.GetQuery<EquivalentStuff>();
      if (id != null)
        equivalentStuff = equivalentStuff.Where(i => i.Id == id);
      if (billOfMaterialDetailId != null)
        equivalentStuff = equivalentStuff.Where(i => i.BillOfMaterialDetailId == billOfMaterialDetailId);
      if (title != null)
        equivalentStuff = equivalentStuff.Where(i => i.Title == title);
      if (description != null)
        equivalentStuff = equivalentStuff.Where(i => i.Description == description);
      if (isActive != null)
        equivalentStuff = equivalentStuff.Where(i => i.IsActive == isActive);
      if (equivalentStuffType != null)
        equivalentStuff = equivalentStuff.Where(i => i.EquivalentStuffType == equivalentStuffType);
      if (billOfMaterialStuffId != null)
        equivalentStuff = equivalentStuff.Where(i => i.BillOfMaterialDetail.BillOfMaterialStuffId == billOfMaterialStuffId);
      if (billOfMaterialVersion != null)
        equivalentStuff = equivalentStuff.Where(i => i.BillOfMaterialDetail.BillOfMaterialVersion == billOfMaterialVersion);
      if (billOfMaterialDetailStuffId != null)
        equivalentStuff = equivalentStuff.Where(i => i.BillOfMaterialDetail.StuffId == billOfMaterialDetailStuffId);
      return equivalentStuff;
    }
    public EquivalentStuff GetEquivalentStuff(int id)
    {
      var equivalentStuff = GetEquivalentStuffs(id: id).SingleOrDefault();
      if (equivalentStuff == null)
        throw new EquivalentStuffNotFoundException(id);
      return equivalentStuff;
    }
    public EquivalentStuff AddEquivalentStuff(int billOfMaterialDetailId, string title, string description, bool isActive, EquivalentStuffType equivalentStuffType)
    {
      var equivalentStuff = repository.Create<EquivalentStuff>();
      equivalentStuff.BillOfMaterialDetailId = billOfMaterialDetailId;
      equivalentStuff.Title = title;
      equivalentStuff.Description = description;
      equivalentStuff.IsActive = isActive;
      equivalentStuff.EquivalentStuffType = equivalentStuffType;
      repository.Add(equivalentStuff);
      return equivalentStuff;
    }
    public EquivalentStuff AddEquivalentStuffProcess(int billOfMaterialDetailId, string title, string description,
        bool isActive, EquivalentStuffType equivalentStuffType, AddEquivalentStuffDetailInput[] equivalentStuffDetails)
    {
      var equivalentStuff = this.AddEquivalentStuff(billOfMaterialDetailId: (int)billOfMaterialDetailId,
                title: (string)title, description: (string)description, isActive: (bool)isActive, equivalentStuffType: (EquivalentStuffType)equivalentStuffType);
      foreach (var equivalentStuffDetail in equivalentStuffDetails)
        AddEquivalentStuffDetail(
                  stuffId: equivalentStuffDetail.StuffId,
                  equivalentStuffId: equivalentStuff.Id,
                  semiProductBillOfMaterialVersion: equivalentStuffDetail.StuffBillOfMaterialVersion,
                  value: equivalentStuffDetail.Value,
                  forQty: equivalentStuffDetail.ForQty,
                  unitId: equivalentStuffDetail.UnitId);
      return equivalentStuff;
    }
    public EquivalentStuff EditEquivalentStuff(byte[] rowVersion, int id,
        TValue<int> billOfMaterialDetailId, TValue<string> title, TValue<string> description, TValue<bool> isActive,
        TValue<EquivalentStuffType> equivalentStuffType)
    {
      var equivalentStuff = GetEquivalentStuff(id: id);
      if (billOfMaterialDetailId != null)
        equivalentStuff.BillOfMaterialDetailId = billOfMaterialDetailId;
      if (title != null)
        equivalentStuff.Title = title;
      if (description != null)
        equivalentStuff.Description = description;
      if (isActive != null)
        equivalentStuff.IsActive = isActive;
      if (equivalentStuffType != null)
        equivalentStuff.EquivalentStuffType = equivalentStuffType;
      repository.Update(entity: equivalentStuff, rowVersion: rowVersion);
      return equivalentStuff;
    }
    public EquivalentStuff EditEquivalentStuffProcess(
        byte[] rowVersion,
        int id,
        TValue<int> billOfMaterialDetailId = null,
        TValue<string> title = null,
        TValue<string> description = null,
        TValue<bool> isActive = null,
        TValue<EquivalentStuffType> equivalentStuffType = null,
        TValue<AddEquivalentStuffDetailInput[]> addEquivalentStuffDetails = null,
        TValue<EditEquivalentStuffDetailInput[]> editEquivalentStuffDetails = null,
        TValue<int[]> deleteEquivalentStuffDetails = null)
    {
      var equivalentStuff = EditEquivalentStuff(
                id: id,
                rowVersion: rowVersion,
                billOfMaterialDetailId: billOfMaterialDetailId,
                title: title,
                description: description,
                isActive: isActive,
                equivalentStuffType: equivalentStuffType);
      if (addEquivalentStuffDetails != null)
        foreach (var equivalentStuffDetail in addEquivalentStuffDetails.Value)
        {
          AddEquivalentStuffDetail(
                        equivalentStuffId: equivalentStuff.Id,
                        stuffId: equivalentStuffDetail.StuffId,
                        semiProductBillOfMaterialVersion: equivalentStuffDetail.StuffBillOfMaterialVersion,
                        value: equivalentStuffDetail.Value,
                        forQty: equivalentStuffDetail.ForQty,
                        unitId: equivalentStuffDetail.UnitId);
        }
      if (editEquivalentStuffDetails != null)
      {
        foreach (var equivalentStuffDetail in editEquivalentStuffDetails.Value)
        {
          EditEquivalentStuffDetail(
                    id: equivalentStuffDetail.Id,
                    rowVersion: equivalentStuffDetail.RowVersion,
                    equivalentStuffId: equivalentStuff.Id,
                    stuffId: equivalentStuffDetail.StuffId,
                    semiProductBillOfMaterialVersion: equivalentStuffDetail.StuffBillOfMaterialVersion ?? new TValue<short?>(null),
                    value: equivalentStuffDetail.Value,
                    forQty: equivalentStuffDetail.ForQty,
                    unitId: equivalentStuffDetail.UnitId);
        }
      }
      if (deleteEquivalentStuffDetails != null)
        foreach (var equivalentStuffDetail in deleteEquivalentStuffDetails.Value)
          DeleteEquivalentStuffDetail(equivalentStuffDetail);
      return equivalentStuff;
    }
    public void DeleteEquivalentStuffProcess(int id)
    {
      var equivalentStuff = GetEquivalentStuff(id: id);
      if (equivalentStuff.EquivalentStuffUsages.Any())
        throw new EquivalentStuffCanNotDeleteHasEquivalentStuffUsageException(equivalentStuff.Id);
      var equivalentStuffDetails = equivalentStuff.EquivalentStuffDetails.ToList();
      foreach (var equivalentStuffDetail in equivalentStuffDetails)
        DeleteEquivalentStuffDetail(equivalentStuffDetail.Id);
      repository.Delete(equivalentStuff);
    }
    public FullEquivalentStuffResult ToFullEquivalentStuffResult(EquivalentStuff equivalentStuff)
    {
      return new FullEquivalentStuffResult
      {
        Id = equivalentStuff.Id,
        Title = equivalentStuff.Title,
        IsActive = equivalentStuff.IsActive,
        Description = equivalentStuff.Description,
        EquivalentStuffType = equivalentStuff.EquivalentStuffType,
        //BillOfMaterialDetailId = equivalentStuff.BillOfMaterialDetail,
        EquivalentStuffDetails = equivalentStuff.EquivalentStuffDetails.ToList().Select(ToFullEquivalentStuffDetailResult).ToArray(),
        RowVersion = equivalentStuff.RowVersion,
      };
    }
    public IQueryable<FullEquivalentStuffResult> SortEquivalentStuffResult(IQueryable<FullEquivalentStuffResult> query, SortInput<EquivalentStuffSortType> sort)
    {
      switch (sort.SortType)
      {
        case EquivalentStuffSortType.Id:
          query = query.OrderBy(x => x.Id, sort.SortOrder);
          break;
        case EquivalentStuffSortType.Title:
          query = query.OrderBy(x => x.Title, sort.SortOrder);
          break;
        case EquivalentStuffSortType.IsActive:
          query = query.OrderBy(x => x.IsActive, sort.SortOrder);
          break;
        case EquivalentStuffSortType.Description:
          query = query.OrderBy(x => x.Description, sort.SortOrder);
          break;
        case EquivalentStuffSortType.EquivalentStuffType:
          query = query.OrderBy(x => x.EquivalentStuffType, sort.SortOrder);
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      return query;
    }
    //public EquivalentStuffResult ToEquivalentStuffResult(EquivalentStuff equivalentStuff)
    //{
    //    return new EquivalentStuffResult
    //    {
    //        Id = equivalentStuff.Id,
    //        Title = equivalentStuff.Title,
    //        IsActive = equivalentStuff.IsActive,
    //        Description = equivalentStuff.Description,
    //        EquivalentStuffType = equivalentStuff.EquivalentStuffType,
    //        //BillOfMaterialDetailId = equivalentStuff.BillOfMaterialDetail,
    //        EquivalentStuffDetails = equivalentStuff.EquivalentStuffDetails.ToList().Select(ToFullEquivalentStuffDetailResult).ToArray(),
    //        RowVersion = equivalentStuff.RowVersion,
    //    };
    //}
  }
}