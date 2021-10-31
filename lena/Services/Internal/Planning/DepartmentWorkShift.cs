using System;
using System.Linq;
using lena.Services.Common;
////using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Planning.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models;
using lena.Models.Common;
using lena.Models.Planning.DepartmentWorkShift;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning
{
  public partial class Planning
  {
    public void DeleteDepartmentWorkShift(int departmentId, int workShiftId)
    {

      var departmentWorkShift = GetDepartmentWorkShift(departmentId: departmentId, workShiftId: workShiftId);
      repository.Delete(departmentWorkShift);
    }
    public IQueryable<DepartmentWorkShift> GetDepartmentWorkShifts(
        TValue<int> departmentId,
        TValue<int> workShiftId)
    {

      var isDepartmentIdNull = departmentId == null;
      var isWorkShiftIdNull = workShiftId == null;
      var workStationparts = from item in repository.GetQuery<DepartmentWorkShift>()
                             where (isDepartmentIdNull || item.DepartmentId == departmentId) &&
                                         (isWorkShiftIdNull || item.WorkShiftId == workShiftId)
                             select item;
      return workStationparts;
    }
    public DepartmentWorkShift GetDepartmentWorkShift(int departmentId, int workShiftId)
    {

      var departmentWorkShift = GetDepartmentWorkShifts(departmentId: departmentId, workShiftId: workShiftId)

            .FirstOrDefault();
      if (departmentWorkShift == null)
        throw new DepartmentWorkShiftNotFoundException(departmentId: departmentId, workShiftId: workShiftId);
      return departmentWorkShift;
    }
    public DepartmentWorkShift AddDepartmentWorkShift(short departmentId, int workShiftId)
    {

      var departmentWorkShift = repository.Create<DepartmentWorkShift>();
      departmentWorkShift.DepartmentId = departmentId;
      departmentWorkShift.WorkShiftId = workShiftId;
      repository.Add(departmentWorkShift);
      return departmentWorkShift;
    }
    public DepartmentWorkShiftResult ToDepartmentWorkShiftResult(DepartmentWorkShift departmentWorkShift)
    {
      return new DepartmentWorkShiftResult
      {
        DepartmentId = departmentWorkShift.DepartmentId,
        DepartmentName = departmentWorkShift.Department.Name,
        WorkShiftId = departmentWorkShift.WorkShiftId,
        WorkShiftName = departmentWorkShift.WorkShift.Name,
        RowVersion = departmentWorkShift.RowVersion,
      };
    }
    public IQueryable<DepartmentWorkShiftResult> ToDepartmentWorkShiftResultQuery(IQueryable<DepartmentWorkShift> query)
    {
      return from departmentWorkShift in query
             select new DepartmentWorkShiftResult
             {
               DepartmentId = departmentWorkShift.DepartmentId,
               DepartmentName = departmentWorkShift.Department.Name,
               WorkShiftId = departmentWorkShift.WorkShiftId,
               WorkShiftName = departmentWorkShift.WorkShift.Name,
               RowVersion = departmentWorkShift.RowVersion,
             };
    }
    public IOrderedQueryable<DepartmentWorkShiftResult> SortDepartmentWorkShiftResult(IQueryable<DepartmentWorkShiftResult> query, SortInput<DepartmentWorkShiftSortType> options)
    {
      switch (options.SortType)
      {
        case DepartmentWorkShiftSortType.DepartmentName:
          return query.OrderBy(a => a.DepartmentName, options.SortOrder);
        case DepartmentWorkShiftSortType.WorkShiftName:
          return query.OrderBy(a => a.WorkShiftName, options.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
  }
}
