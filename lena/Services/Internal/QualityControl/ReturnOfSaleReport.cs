//using lena.Services.Core.Foundation.Service.Internal.Action;
using System;
using System.Collections.Generic;
using System.Data.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityControl
{
  public partial class QualityControl
  {
    public IEnumerable<ExpandoObject> GetReturnOfSaleReport(
        List<string> years,
        List<string> yearMonths,
        int? stuffId,
        int cooperatorId,
        bool isMonthly)
    {

      if (years.Any() && yearMonths.Any())
      {
        yearMonths.Clear();
      }
      string joinedYears = string.Join("@", years);
      string joinedYearMonths = string.Join("@", yearMonths);


      var parameters = new List<SqlParameter>();

      if (years.Any())
        parameters.Add(new SqlParameter() { ParameterName = "@years", Value = joinedYears });
      else
        parameters.Add(new SqlParameter() { ParameterName = "@years", Value = DBNull.Value });

      if (yearMonths.Any())
        parameters.Add(new SqlParameter() { ParameterName = "@yearMonthly", Value = joinedYearMonths });
      else
        parameters.Add(new SqlParameter() { ParameterName = "@yearMonthly", Value = DBNull.Value });

      parameters.Add(new SqlParameter() { ParameterName = "@isMonthly", Value = isMonthly });
      parameters.Add(new SqlParameter() { ParameterName = "@cooperatorId", Value = cooperatorId });

      if (stuffId != null)
        parameters.Add(new SqlParameter() { ParameterName = "@stuffId", Value = stuffId });
      else
        parameters.Add(new SqlParameter() { ParameterName = "@stuffId", Value = DBNull.Value });


      var result = repository.CreateQueryWithDynamicResult("EXEC [dbo].[ReturnOfSalesReport] @years, @yearMonthly, @isMonthly, @cooperatorId, @stuffId", parameters.ToArray<DbParameter>());

      return result.ToList();
    }
  }
}
