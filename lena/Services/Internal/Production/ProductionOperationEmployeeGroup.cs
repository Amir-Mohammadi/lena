//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Production.Exception;
using lena.Models.Common;
using lena.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using lena.Services.Core;
using Newtonsoft.Json;
using lena.Services.Core.Common;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Production
{
  public partial class Production
  {
    #region Add
    public ProductionOperationEmployeeGroup GetOrAddProductionOperationEmployeeGroup(int[] employeeIds)
    {

      var sortedList = employeeIds.OrderBy(i => i);
      var jsonVlaue = JsonConvert.SerializeObject(sortedList);
      string HashedEmployee = Crypto.Sha1(jsonVlaue);
      var productionOperationEmployeeGroup = repository.GetQuery<ProductionOperationEmployeeGroup>()
            .FirstOrDefault(i => i.HashedEmployee == HashedEmployee);
      if (productionOperationEmployeeGroup == null)
      {
        productionOperationEmployeeGroup = repository.Create<ProductionOperationEmployeeGroup>();
        productionOperationEmployeeGroup.HashedEmployee = HashedEmployee;
        repository.Add(productionOperationEmployeeGroup);
        foreach (var employeeId in employeeIds)
        {
          AddProductionOperationEmployee(employeeId: employeeId,
                    productionOperationEmployeeGroupId: productionOperationEmployeeGroup.Id);
        }
      }
      return productionOperationEmployeeGroup;
    }
    #endregion
  }
}
