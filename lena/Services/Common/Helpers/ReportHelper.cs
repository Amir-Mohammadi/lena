using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using lena.Services.Core;
using lena.Models.Reports;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.TextFormats;
namespace lena.Services.Common.Helpers
{
  public class ReportHelper
  {
    private static StiReport GenerateStiReport(Type type)
    {
      #region GenerateReport
      var report = GenerateBaseReport(null);
      #endregion
      #region GenereatePage
      var page = report.Pages[0];
      #endregion
      #region Generate Components
      if (type.GetInterfaces().Contains(typeof(IEnumerable)))
      {
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
        #region Generate Components
        var genericType = type.GenericTypeArguments[0];
        var properties = genericType.GetProperties();
        var datePropCount = properties.Count(i => i.PropertyType == typeof(DateTime));
        double position = 0;
        double columnWidth = StiAlignValue.AlignToMinGrid(page.Width / (properties.Length + datePropCount), 0.1, true);
        int nameIndex = 1;
        var hasDateTimeProp = false;
        foreach (var property in properties)
        {
          StiFormatService stiFormat = null;
          if (property.Name == "RowVersion")
            continue;
          if (CommonHelper.IsNumericType(property.PropertyType))
            stiFormat = new Stimulsoft.Report.Components.TextFormats.StiNumberFormatService(1, ".", 2, ",", 3, true, true, " ");
          hasDateTimeProp = false;
          if (property.PropertyType == typeof(DateTime))
          {
            hasDateTimeProp = true;
            var headerTextDate = CreateHeaderText(propertyName: property.Name, position: position, columnWidth: columnWidth, nameIndex: nameIndex);
            headerBand.Components.Add(headerTextDate);
            var stiFormatDate = new Stimulsoft.Report.Components.TextFormats.StiDateFormatService("yyyy/MM/dd", " ");
            var dataTextDate = CreateDataText(propertyName: property.Name, position: position, columnWidth: columnWidth, nameIndex: nameIndex, stiFormat: stiFormatDate);
            dataBand.Components.Add(dataTextDate);
            nameIndex++;
            position += columnWidth;
            var headerTextTime = CreateHeaderText(propertyName: property.Name, position: position, columnWidth: columnWidth, nameIndex: nameIndex);
            headerBand.Components.Add(headerTextTime);
            var stiFormatTime = new Stimulsoft.Report.Components.TextFormats.StiTimeFormatService("HH:mm");
            var dataTextTime = CreateDataText(propertyName: property.Name, position: position, columnWidth: columnWidth, nameIndex: nameIndex, stiFormat: stiFormatTime);
            dataBand.Components.Add(dataTextTime);
          }
          else
          {
            var headerText = CreateHeaderText(propertyName: property.Name, position: position, columnWidth: columnWidth, nameIndex: nameIndex);
            headerBand.Components.Add(headerText);
            var dataText = CreateDataText(propertyName: property.Name, position: position, columnWidth: columnWidth, nameIndex: nameIndex, stiFormat: stiFormat);
            dataBand.Components.Add(dataText);
          }
          nameIndex++;
          position += columnWidth;
        }
        #endregion
      }
      else
      {
        #region  Create Report Components  for Objects
        var reportComponents = GenerateComponents(type);
        var startY = 0.5 + 0.5;
        var standardTxtWidth = 1;
        var groupedResult = reportComponents.GroupBy(x => x.ParentName).ToList();
        groupedResult.ForEach(x =>
        {
          var startX = 0.0;
          var items = x.ToList();
          var txtWidth = (page.Width / items.Count);
          var txtBoxWidth = StiAlignValue.AlignToMinGrid(txtWidth >= standardTxtWidth ? txtWidth : standardTxtWidth, 0.1,
                      true);
          var pageWidth = StiAlignValue.AlignToMinGrid(page.Width - page.Margins.Right, 0.1, true);
          items.ForEach(i =>
                  {
                    i.HeaderText.Left = startX;
                    i.HeaderText.Top = startY;
                    i.HeaderText.Width = txtBoxWidth;
                    i.HeaderText.Height = 0.5;
                    i.DataText.Left = startX;
                    i.DataText.Top = startY + 0.5;
                    i.DataText.Width = txtBoxWidth;
                    i.DataText.Height = 0.5;
                    if (startX + txtBoxWidth > pageWidth)
                    {
                      startY += (2 * 0.5);
                      startX = 0;
                    }
                    else
                      startX += txtBoxWidth;
                    page.Components.Add(i.HeaderText);
                    page.Components.Add(i.DataText);
                  });
          startY += 2 * 0.5;
        });
        #endregion
      }
      #endregion
      return report;
    }
    public static StiComponent CreateHeaderText(string propertyName, double position, double columnWidth, int nameIndex)
    {
      StiText headerText = new StiText(new RectangleD(position, 0, columnWidth, 0.5));
      headerText.HorAlignment = StiTextHorAlignment.Center;
      headerText.Name = "HeaderText" + nameIndex.ToString();
      headerText.Border.Side = StiBorderSides.All;
      headerText.Font = new Font("Tahoma", 10);
      headerText.CanGrow = false;
      headerText.CanBreak = false;
      headerText.CanShrink = false;
      headerText.Text = propertyName + nameIndex;
      return headerText;
    }
    public static StiComponent CreateDataText(string propertyName, double position, double columnWidth, int nameIndex, StiFormatService stiFormat)
    {
      StiText dataText = new StiText(new RectangleD(position, 0, columnWidth, 0.5));
      dataText.HorAlignment = StiTextHorAlignment.Center;
      dataText.Text.Value =
          "{DT." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(propertyName) + "}";
      dataText.Name = "DataText" + nameIndex.ToString();
      dataText.Border.Side = StiBorderSides.All;
      dataText.Font = new Font("Tahoma", 10);
      dataText.CanGrow = false;
      dataText.CanBreak = false;
      dataText.CanShrink = false;
      if (stiFormat != null)
        dataText.TextFormat = stiFormat;
      StiCondition condition = new StiCondition();
      condition.BackColor = Color.White;
      condition.TextColor = Color.Black;
      condition.Expression = "(Line & 1) == 1";
      condition.Item = StiFilterItem.Expression;
      dataText.Conditions.Add(condition);
      return dataText;
    }
    public static StiReport RegisterData(StiReport report, object data, KeyValueInput[] parameters)
    {
      var listData = data as IEnumerable<dynamic>;
      if (listData != null)
      {
        report.RegData("DT", listData);
        report.Dictionary.Synchronize();
      }
      else
      {
        report.RegBusinessObject("DT", data);
        report.Dictionary.SynchronizeBusinessObjects(5);
      }
      if (parameters != null)
        foreach (var parameter in parameters)
          report.Dictionary.Variables.Add(parameter.Key, parameter.Value);
      return report;
    }
    public static StiReport GenerateReport(Type dataType, KeyValueInput[] parameters)
    {
      Stimulsoft.Report.StiReport report;
      report = GenerateStiReport(dataType);
      var sampleData = Activator.CreateInstance(dataType);
      RegisterData(report: report, data: sampleData, parameters: parameters);
      report.Compile();
      report.Render(false);
      return report;
    }
    public static StiReport GenerateBaseReport(string cultureName)
    {
      var report = new StiReport();
      report.AutoLocalizeReportOnRun = true;
      report.Culture = !string.IsNullOrEmpty(cultureName) ? cultureName : "fa-IR";
      report.CalculationMode = StiCalculationMode.Compilation;
      report.Dictionary.Variables.Add("UserName", App.Providers.Security.CurrentLoginData.UserFullName);
      var referencedAssemblies = report.ReferencedAssemblies.ToList();
      referencedAssemblies.Add("Parlar.Model.Dll");
      referencedAssemblies.Add("System.Core.Dll");
      report.ReferencedAssemblies = referencedAssemblies.ToArray();
      var fontsDir = AssetsHelper.GetAllFonts().ToList();
      fontsDir.ForEach(Stimulsoft.Base.StiFontCollection.AddFontFile);
      return report;
    }
    private static StiText GetStandardStimulText(string name, PointD location, SizeD size, bool fillBackColor = false)
    {
      StiText txt = new StiText(new RectangleD(location, size));
      //headerText.Text.Value = dataColumn.Caption;
      txt.HorAlignment = StiTextHorAlignment.Center;
      txt.VertAlignment = StiVertAlignment.Center;
      txt.Border = new StiBorder(StiBorderSides.All, Color.FromArgb(255, 155, 155, 155), 1, StiPenStyle.Solid, false, 4, new StiSolidBrush(Color.Black), false);
      if (fillBackColor)
        txt.Brush = new StiSolidBrush(Color.FromArgb(255, 221, 217, 195));
      txt.Name = name;
      txt.Border.Side = StiBorderSides.All;
      txt.Font = new Font("IRANSans", 10);
      txt.CanGrow = false;
      txt.CanBreak = false;
      txt.CanShrink = false;
      txt.Text = name;
      return txt;
    }
    private static IEnumerable<ReportComponent> GenerateComponents(Type type, string parent = null)
    {
      if (type.IsGenericType)
        type = type.GenericTypeArguments[0];
      var properties = type.GetProperties();
      var stiTxtList = new List<ReportComponent>();
      string parentName = !string.IsNullOrEmpty(parent) ? parent + "." : "DT.";
      foreach (var prop in properties)
      {
        var propValueType = prop.PropertyType;
        string objPath = parentName + prop.Name;
        //Prevent loop for parent/child classes
        if (propValueType.IsClass && prop.PropertyType.Assembly.FullName == type.Assembly.FullName && prop.Name != type.Name)
          stiTxtList.AddRange(GenerateComponents(prop.PropertyType, objPath));
        else
        {
          StiText headerText = GetStandardStimulText(prop.Name, new PointD(0, 0), new SizeD(0, 0), true);
          headerText.Name = "HeaderText_" + prop.Name;
          headerText.CanBreak = false;
          headerText.CanGrow = false;
          headerText.CanShrink = false;
          StiText dataText = GetStandardStimulText(prop.Name, new PointD(0, 0), new SizeD(0, 0));
          dataText.Text.Value = "{" + parentName + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(prop.Name) + "}";
          dataText.Name = "DataText_" + prop.Name;
          stiTxtList.Add(new ReportComponent(parentName, headerText, dataText));
        }
      }
      return stiTxtList;
    }
  }
  public class ReportComponent
  {
    public ReportComponent(string parentName,
        StiText headerText,
        StiText dataText)
    {
      this.ParentName = parentName;
      this.HeaderText = headerText;
      this.DataText = dataText;
    }
    public string ParentName { get; set; }
    public StiText HeaderText { get; set; }
    public StiText DataText { get; set; }
  }
}