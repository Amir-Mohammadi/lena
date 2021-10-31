using lena.Services.Common.Helpers;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Models.QualityAssurance.EmployeeEvaluationItemDetail;
using lena.Models.QualityAssurance.EmployeeEvaluationReport;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using System.Drawing;
using System.Dynamic;
using System.IO;
using System.Linq;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityAssurance
{
  public partial class QualityAssurance
  {
    public IEnumerable<ExpandoObject> GetEmployeeEvaluationReport(
      int[] employeeEvaluationIds,
      int? departmentId,
      int? employeeId)
    {


      string joinedemployeeEvaluationIds = string.Join(",", employeeEvaluationIds);

      var parameters = new List<SqlParameter>();
      parameters.Add(new SqlParameter() { ParameterName = "@evaluationPeriodIds", Value = joinedemployeeEvaluationIds });


      if (employeeId != null)
        parameters.Add(new SqlParameter() { ParameterName = "@employeeId ", Value = employeeId });
      else
        parameters.Add(new SqlParameter() { ParameterName = "@employeeId ", Value = DBNull.Value });

      if (departmentId != null)
        parameters.Add(new SqlParameter() { ParameterName = "@departmentId", Value = departmentId });
      else
        parameters.Add(new SqlParameter() { ParameterName = "@departmentId", Value = DBNull.Value });


      var result = repository.CreateQueryWithDynamicResult("EXEC [dbo].[sp_EmployeeEvaluation] @evaluationPeriodIds, @employeeId, @departmentId", parameters.ToArray<DbParameter>());

      return result.ToList();
    }

    public byte[] GetEmployeeEvaluationReportExcel(
      int[] employeeEvaluationIds,
      int? departmentId,
      int? employeeId)
    {

      var dynamicData = GetEmployeeEvaluationReport(
                employeeEvaluationIds: employeeEvaluationIds,
                departmentId: departmentId,
                employeeId: employeeId);

      //Generate DataSet
      var propsMap = GetEmployeeEvaluationPropertiesMap();
      var empEvlDataSet = GenerateReportWithDyanmicData.GenerateDataSet(dynamicData: dynamicData, propsMap: propsMap);
      //Generate Report 

      return GenerateReportWithDyanmicData.GenerateReport(dataSet: empEvlDataSet);
    }



    public IQueryable<EmployeeEvaluationItemDetailResult> GetEmployeeEvaluationReportDetails(int employeeId, string employeeEvaluationPeriodTitle)
    {


      var employeeEvaluationPeriod = GetEmployeeEvaluationPeriodByName(
                title: employeeEvaluationPeriodTitle);

      var employeeEvaluationPeriodId = employeeEvaluationPeriod?.Id ?? null;
      var employeeEvaluations = GetEmployeeEvaluations(
                selector: e => e,
                employeeId: employeeId,
                employeeEvaluationPeriodId: employeeEvaluationPeriodId);


      var employeeEvaluationId = employeeEvaluations.FirstOrDefault()?.Id ?? null;
      var employeeEvaluationItemDetails = GetEmployeeEvaluationItemDetails(
                selector: e => e,
                employeeEvaluationId: employeeEvaluationId);

      var employeeEvaluationPeriodItems = GetEmployeeEvaluationPeriodItems(
                selector: e => e,
                employeeEvaluationPeriodId: employeeEvaluationPeriodId);





      var result = from employeeEvaluationItemDetail in employeeEvaluationItemDetails
                   join employeeEvaluation in employeeEvaluations on employeeEvaluationItemDetail.EmployeeEvaluationId equals employeeEvaluation.Id
                   join employeeEvaluationPeriodItem in employeeEvaluationPeriodItems
                          on new { employeeEvaluationItemDetail.EvaluationCategoryId, employeeEvaluation.EmployeeEvaluationPeriodId }
                          equals
                         new { employeeEvaluationPeriodItem.EvaluationCategoryId, employeeEvaluationPeriodItem.EmployeeEvaluationPeriodId }
                   select new EmployeeEvaluationItemDetailResult
                   {
                     EmployeeEvaluationId = employeeEvaluationItemDetail.EmployeeEvaluationId,
                     EvaluationCategoryId = employeeEvaluationItemDetail.EvaluationCategoryId,
                     EvaluationScore = employeeEvaluationItemDetail.Score,
                     EvaluationCategoryItemId = employeeEvaluationItemDetail.EvaluationCategoryItemId,
                     EvaluationCategoryTitle = employeeEvaluationItemDetail.EvaluationCategoryItem.EvaluationCategory.Title,
                     EmployeeEvaluationItemDescription = employeeEvaluationItemDetail.EmployeeEvaluationItem.Description,
                     EmployeeEvaluationItemDateTime = employeeEvaluationItemDetail.EmployeeEvaluationItem.DateTime,
                     EvaluationQuestion = employeeEvaluationItemDetail.EvaluationCategoryItem.Question,
                     Coefficient = employeeEvaluationPeriodItem.Coefficient,
                     EmployeeEvaluationItemEmployeeFullName = employeeEvaluationItemDetail.EmployeeEvaluationItem.User.Employee.FirstName + " " + employeeEvaluationItemDetail.EmployeeEvaluationItem.User.Employee.LastName,
                   };



      return result;
    }


    private IDictionary<string, string> GetEmployeeEvaluationPropertiesMap()
    {
      var empEvalPropsMap = new Dictionary<string, string>();

      var resultFixedProps = typeof(EmployeeEvaluationReportRawResult).GetProperties();

      foreach (var prop in resultFixedProps)
      {
        empEvalPropsMap[prop.Name] = employeeEvaluationPropertiesMap(prop: prop.Name);
      }

      return empEvalPropsMap;
    }

    private string employeeEvaluationPropertiesMap(string prop)
    {
      switch (prop)
      {
        case "EmployeeCode":
          return "کد پرسنلی";
        case "EmployeeFullName":
          return "پرسنل";
        default:
          return string.Empty;
      }
    }
  }

  public static class GenerateReportWithDyanmicData
  {
    private static DataSet dataSet;

    public static DataSet GenerateDataSet(IEnumerable<ExpandoObject> dynamicData, IDictionary<string, string> propsMap)
    {
      dataSet = new DataSet();
      var dataTable = dynamicData.FirstOrDefault().AddDataTableColumns(propsMap: propsMap);
      dataTable = dataTable.AddDataTableRows(dynamicData: dynamicData, propsMap: propsMap);
      dataSet.Tables.Add(dataTable);
      return dataSet;
    }

    private static DataTable AddDataTableColumns(this ExpandoObject rowData, IDictionary<string, string> propsMap)
    {
      var dataTable = new DataTable("DT");
      if (rowData == null)
        return dataTable;
      var rowProps = (rowData as IDictionary<string, object>).ToArray();

      foreach (var item in rowProps)
      {
        var mappedValue = true;
        if (propsMap.ContainsKey(item.Key) && string.IsNullOrEmpty(propsMap[item.Key]))
        {
          mappedValue = false;
        }
        if (mappedValue)
        {

          if (propsMap.ContainsKey(item.Key))
          {
            dataTable.Columns.Add(new DataColumn(propsMap[item.Key], typeof(string)));
          }
          else
          {
            dataTable.Columns.Add(new DataColumn(item.Key, typeof(float)));
          }
        }

      }
      return dataTable;
    }

    private static DataTable AddDataTableRows(this DataTable dataTable, IEnumerable<ExpandoObject> dynamicData, IDictionary<string, string> propsMap)
    {
      foreach (var item in dynamicData)
      {
        var dic = (item as IDictionary<string, object>).ToArray();

        var myDataRow = dataTable.NewRow();


        foreach (var subItem in dic)
        {
          var mappedValue = true;
          if (propsMap.ContainsKey(subItem.Key) && string.IsNullOrEmpty(propsMap[subItem.Key]))
          {
            mappedValue = false;
          }
          if (mappedValue)
          {

            if (propsMap.ContainsKey(subItem.Key))

            {
              myDataRow[propsMap[subItem.Key]] = subItem.Value.ToString();

            }

            else
            {
              float result;
              var tryParse = float.TryParse(subItem.Value.ToString(), out result);
              myDataRow[subItem.Key] = result;

            }

          }
        }

        dataTable.Rows.Add(myDataRow);
      }
      return dataTable;
    }
    public static byte[] GenerateReport(DataSet dataSet)
    {

      #region Generate Base Report
      var report = ReportHelper.GenerateBaseReport(null);
      var stream = new MemoryStream();
      #endregion

      #region Synchronize Dictionary
      report.RegData(dataSet);
      report.Dictionary.Synchronize();
      #endregion

      #region Create Report Option
      StiPage page = report.Pages[0];
      #region Generate HeaderBand
      var headerBand = new StiPageHeaderBand();
      headerBand.Height = 0.5;
      headerBand.Name = "HeaderBand";
      headerBand.SetFont(new Font("IRANSans", 12));
      headerBand.CanGrow = true;
      page.Components.Add(headerBand);
      #endregion

      #region Generate DataBand
      var dataBand = new StiDataBand();
      dataBand.DataSourceName = "DT";
      dataBand.Height = 0.5;
      dataBand.Name = "DataBand";
      dataBand.SetFont(new Font("IRANSans", 10));
      dataBand.CanGrow = true;
      page.Components.Add(dataBand);
      #endregion
      double position = 0;
      double columnWidth = 2;
      double pageWidth = 4;
      foreach (var header in dataSet.Tables["DT"].Columns)
      {

        StiText headerText = new StiText(new RectangleD(position, 0, columnWidth, 0.5));
        headerText.HorAlignment = StiTextHorAlignment.Center;
        headerText.Name = "HeaderText" + position.ToString();
        headerText.Border.Side = StiBorderSides.All;
        headerText.Font = new Font("Tahoma", 12, FontStyle.Bold); //new Font("Tahoma", 10);
        headerText.CanGrow = false;
        headerText.CanBreak = false;
        headerText.CanShrink = false;
        headerText.Text = header.ToString();
        headerBand.Components.Add(headerText);

        StiText dataText = new StiText(new RectangleD(position, 0, columnWidth, 0.5));
        dataText.HorAlignment = StiTextHorAlignment.Center;
        dataText.Text.Value =
            "{DT." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(header.ToString()) + "}";
        dataText.Name = "DataText" + position.ToString();
        dataText.Border.Side = StiBorderSides.All;
        dataText.Font = new Font("Tahoma", 12, FontStyle.Regular);
        dataText.CanGrow = false;
        dataText.CanBreak = false;
        dataText.CanShrink = false;
        StiCondition condition = new StiCondition();
        condition.BackColor = Color.White;
        condition.TextColor = Color.Black;
        condition.Expression = "(Line & 1) == 1";
        condition.Item = StiFilterItem.Expression;
        dataText.Conditions.Add(condition);
        dataBand.Components.Add(dataText);


        position += columnWidth;
        pageWidth += columnWidth;
      }
      page.PageWidth = pageWidth;

      page.PageHeight = 1000;

      StiOptions.Export.Excel.ColumnsRightToLeft = true;
      report.Compile();
      report.Render(false);
      report.ExportDocument(StiExportFormat.Excel, stream);
      return stream.ToArray();
      #endregion


    }


  }

}

