using System.Linq;
using lena.Services.Core;

using lena.Models.Common;
using lena.Domains;
using lena.Models.Unit;
using lena.Models.WarehouseManagement.Common;
using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {
    public IQueryable<UnitComboResult> GetStuffUnits(int stuffId)
    {

      var unitComboResults = App.Internals.ApplicationBase.GetUnits(
                selector: App.Internals.ApplicationBase.ToUnitComboResult,
                stuffId: stuffId);
      return unitComboResults;
    }
    public UnitResult GetStuffMainUnit(int stuffId)
    {

      var unitComboResults = App.Internals.ApplicationBase.GetUnits(
                selector: App.Internals.ApplicationBase.ToUnitResult,
                stuffId: stuffId,
                isMainUnit: true);
      return unitComboResults.FirstOrDefault();
    }
  }
}